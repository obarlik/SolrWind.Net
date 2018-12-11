using System;
using System.Net;
using System.Text;

namespace SolrWind.Net
{
    internal class SolrClient : WebClient
    {
        public SolrClient()
        {
        }


        public string UploadJson(Uri address, string data)
        {
            Headers["Content-Type"] = "application/json";
            Encoding = Encoding.UTF8;

            return UploadString(address, data);
        }


        public string UploadJson(Uri address, JsonObject obj)
        {
            return UploadJson(address, obj.ToJson());
        }
    }
}