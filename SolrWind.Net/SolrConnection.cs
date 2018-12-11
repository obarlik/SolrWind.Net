using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolrWind.Net
{
    public class SolrConnection
    {
        public SolrConnection(SolrConnector connector, string collectionName)
        {
            Connector = connector;
            CollectionName = collectionName;
            Client = connector.NewClient();
        }
        

        public SolrConnector Connector { get; }
        public string CollectionName { get; }

        WebClient Client;


        public async Task Pump(IEnumerable source, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var i = 0;

                try
                {
                    foreach (var item in source)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (++i % 1000 == 0)
                            Commit();

                        Update(new SolrUpdate(item));
                    }
                }
                catch
                {
                    Rollback();
                    throw;
                }

                Commit();
                Optimize();
            },
            cancellationToken);
        }


        Uri _UpdateUri;

        Uri UpdateUri
        {
            get
            {
                return _UpdateUri ??
                      (_UpdateUri = new Uri(Connector.NewCollectionUri(CollectionName) + "/update"));
            }
        }


        void Commit()
        {
            Update(new SolrCommit());
        }


        void Optimize()
        {
            Update(new SolrOptimize());
        }


        void Rollback()
        {
            Update(new SolrRollback());
        }


        void Update(JsonObject obj)
        {
            Client.Headers["Content-Type"] = "application/json";
            Client.Encoding = Encoding.UTF8;
            var json = obj.ToJson(Debugger.IsAttached);

            Debug.WriteLineIf(Debugger.IsAttached,
                "Posting to " + UpdateUri 
              + "\nData\n----------\n" + json);

            Debug.Indent();

            try
            {
                var result = Client.UploadString(UpdateUri, json);

                Debug.WriteLineIf(Debugger.IsAttached,
                    "Result\n------------\n" + result);
            }
            finally
            {
                Debug.Unindent();
            }
        }
    }
}
