namespace SolrWind.Net
{
    internal partial class SolrAdd : JsonObject
    {
        public SolrDocument add = new SolrDocument();

        public SolrAdd(object document)
        {
            add.doc = document;
        }
    }
}
