namespace SolrWind.Net
{
    public class SolrParams : JsonObject
    {
        public bool waitSearcher = true;
        public bool expungeDeletes = false;
        public int maxSegments = 1;
    }
}
