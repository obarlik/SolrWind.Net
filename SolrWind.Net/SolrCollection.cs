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


        public async Task DataPump(IEnumerable source, CancellationToken cancellationToken, Action<object, int> OnProgress = null)
        {
            var i = 0;

            try
            {
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (++i % 1000 == 0)
                        await CommitAsync();

                    OnProgress?.Invoke(item, i);

                    await UpdateAsync(item);
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }

            await CommitAndOptimizeAsync();
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



        Uri _UpdateUri;

        Uri UpdateUri
        {
            get
            {
                return _UpdateUri ??
                      (_UpdateUri = new Uri(CollectionUri + "/update"));
            }
        }

        
        public string Commit()
        {
            return Post(new SolrCommit());
        }


        public async Task<string> CommitAsync()
        {
            return await PostAsync(new SolrCommit());
        }


        public string Optimize()
        {
            return Post(new SolrOptimize());
        }


        public async Task<string> OptimizeAsync()
        {
            return await PostAsync(new SolrOptimize());
        }


        public string CommitAndOptimize()
        {
            Commit();
            return Optimize();
        }


        public async Task<string> CommitAndOptimizeAsync()
        {
            await CommitAsync();
            return await OptimizeAsync();
        }
        

        public string Update(object item)
        {
            return Post(new SolrAdd(item));
        }
        

        public async Task<string> UpdateAsync(object item)
        {
            return await PostAsync(new SolrAdd(item));
        }


        public string DeleteSingle(string id)
        {
            return Post(new SolrDeleteSingle(id));
        }


        public async Task<string> DeleteSingleAsync(string id)
        {
            return await PostAsync(new SolrDeleteSingle(id));
        }


        public string DeleteQuery(string query)
        {
            return Post(new SolrDeleteQuery(query));
        }


        public async Task<string> DeleteQueryAsync(string query)
        {
            return await PostAsync(new SolrDeleteQuery(query));
        }


        public string Rollback()
        {
            return Post(new SolrRollback());
        }


        public async Task<string> RollbackAsync()
        {
            return await PostAsync(new SolrRollback());
        }


        async Task<string> PostAsync(JsonObject obj)
        {
            return await new SolrClient().UploadJsonAsync(UpdateUri, obj);
        }


        string Post(JsonObject obj)
        {
            return new SolrClient().UploadJson(UpdateUri, obj);
        }
    }
}
