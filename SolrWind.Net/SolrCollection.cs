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
    public class SolrCollection
    {
        public SolrCollection(SolrService connector, string collectionName)
        {
            Connector = connector;
            CollectionName = collectionName;
            Client = new SolrClient();
        }
        

        public SolrService Connector { get; }
        public string CollectionName { get; }

        SolrClient Client;


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

                        Update(item);
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


        Uri _CollectionUri;

        public Uri CollectionUri
        {
            get
            {
                return _CollectionUri ??
                      (_CollectionUri = Connector.NewCollectionUri(CollectionName));
            }
        }

        
        Uri _UpdateUri;

        Uri UpdateUri
        {
            get
            {
                return _UpdateUri ??
                      (_UpdateUri = new Uri(CollectionUri + "/update"));
            }
        }


        string Commit()
        {
            return Post(new SolrCommit());
        }


        string Optimize()
        {
            return Post(new SolrOptimize());
        }


        string Update(object item)
        {
            return Post(new SolrUpdate(item));
        }


        string Rollback()
        {
            return Post(new SolrRollback());
        }


        string Post(JsonObject obj)
        {
            return Client.UploadJson(UpdateUri, obj);
        }
    }
}
