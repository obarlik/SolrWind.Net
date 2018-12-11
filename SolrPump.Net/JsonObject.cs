﻿using Newtonsoft.Json;

namespace SolrPump.Net
{
    public class JsonObject
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
