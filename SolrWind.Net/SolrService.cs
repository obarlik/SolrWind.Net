using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SolrWind.Net
{
    public class SolrService
    {
        public string Host;
        public int Port;
        public bool UseSsl;
        
        public SolrService(string host = "localhost", int port = 8983, bool useSsl = false)
        {
            Host = host;
            Port = port;
            UseSsl = useSsl;
        }


        string _BaseAddress;

        public string BaseAddress
        {
            get
            {
                return _BaseAddress ??
                      (_BaseAddress = 
                            string.Format(
                                "{0}://{1}:{2}/solr", 
                                (UseSsl ? "https" : "http"), Host, Port));
            }
        }
        

        public SolrCollection GetCollection(string collectionName)
        {
            return new SolrCollection(this, collectionName);
        }
    }
}
