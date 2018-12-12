namespace SolrWind.Net
{
    internal class SolrDeleteSingle : JsonObject
    {
        public SolrDeleteSingle(string id)
        {
            delete = new SolrSingle(id);
        }

        public SolrSingle delete;

    }
}
