namespace SolrWind.Net
{
    public class SolrUpdate : JsonObject
    {
        public class SolrAdd : JsonObject
        {
            public bool overwrite = true;
            public object doc;
        }

        public SolrAdd add = new SolrAdd();

        public SolrUpdate(object document)
        {
            add.doc = document;
        }
    }
}
