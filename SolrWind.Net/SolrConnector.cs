using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SolrWind.Net
{
    public class SolrConnector
    {
        public string Host = "localhost";
        public int Port = 8983;
        public bool UseSsl = false;

        public string BaseAddress
        {
            get { return (UseSsl ? "https" : "http") + "://" + Host + ":" + Port + "/solr/"; }
        }


        public WebClient NewClient()
        {
            return new WebClient()
            {
                BaseAddress = this.BaseAddress
            };
        }


        public Uri NewCollectionUri(string collectionName)
        {
            return new Uri(new Uri(BaseAddress), collectionName);
        }


        public SolrConnection NewConnection(string collectionName)
        {
            return new SolrConnection(this, collectionName);
        }
    }
}
