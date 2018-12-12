namespace SolrWind.Net
{
    internal class SolrDeleteQuery : JsonObject
    {
        public SolrDeleteQuery(string query)
        {
            delete = new SolrQuery(query);
        }

        public SolrQuery delete;

    }
}
