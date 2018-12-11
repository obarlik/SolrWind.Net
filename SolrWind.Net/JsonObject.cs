using Newtonsoft.Json;

namespace SolrWind.Net
{
    internal class JsonObject
    {
        public string ToJson(bool indented = false)
        {
            return JsonConvert.SerializeObject(
                this, 
                indented ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings()
            {
                DateFormatString = "yyyy'-'MM'-'ddTHH':'mm':'ssZ"
            });
        }
    }
}
