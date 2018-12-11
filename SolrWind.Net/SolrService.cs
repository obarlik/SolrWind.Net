using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SolrWind.Net
{
    public class SolrService
    {
        public string Host = "localhost";
        public int Port = 8983;
        public bool UseSsl = false;

        public string BaseAddress
        {
            get { return (UseSsl ? "https" : "http") + "://" + Host + ":" + Port + "/solr/"; }
        }
        

        public Uri NewCollectionUri(string collectionName)
        {
            return new Uri(new Uri(BaseAddress), collectionName);
        }


        public SolrCollection GetCollection(string collectionName)
        {
            return new SolrCollection(this, collectionName);
        }
    }
}
