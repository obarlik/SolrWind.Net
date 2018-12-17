using Json.Net;
using System;
using System.Globalization;

namespace SolrWind.Net
{
    internal class JsonObject
    {
        public string ToJson(bool indented = false)
        {
            return JsonNet.Serialize(
                this, 
                new JsonConverter<DateTime>(
                    dt => dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture),
                    s => DateTime.ParseExact(s, "yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture)));
        }
    }
}
