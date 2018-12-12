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
        }
        

        public SolrService Connector { get; }
        public string CollectionName { get; }


        public async Task Pump(IEnumerable source, CancellationToken cancellationToken)
        {
            var i = 0;

            try
            {
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (++i % 1000 == 0)
                        await Commit();

                    await Update(item);
                }
            }
            catch
            {
                await Rollback();
                throw;
            }

            await CommitAndOptimize();
        }


        Uri _CollectionUri;

        public Uri CollectionUri
        {
            get
            {
                return _CollectionUri ??
                      (_CollectionUri = new Uri(Connector.BaseAddress + "/" + CollectionName));
            }
        }


        public async Task<string> DeleteSingle(string id)
        {
            return await Post(new SolrDeleteSingle(id));
        }


        public async Task<string> DeleteQuery(string query)
        {
            return await Post(new SolrDeleteQuery(query));
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


        public async Task<string> Commit()
        {
            return await Post(new SolrCommit());
        }


        public async Task<string> Optimize()
        {
            return await Post(new SolrOptimize());
        }


        public async Task<string> CommitAndOptimize()
        {
            await Commit();
            return await Optimize();
        }


        public async Task<string> Update(object item)
        {
            return await Post(new SolrAdd(item));
        }


        public async Task<string> Rollback()
        {
            return await Post(new SolrRollback());
        }


        async Task<string> Post(JsonObject obj)
        {
            return await new SolrClient().UploadJsonAsync(UpdateUri, obj);
        }
    }
}
