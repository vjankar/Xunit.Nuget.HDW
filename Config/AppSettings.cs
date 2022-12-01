using Newtonsoft.Json;

namespace selenium.xunit.framework.package.Config
{
    [JsonObject("appSettings")]
    public class AppSettings
    {
        [JsonProperty("webDriver")]
        public string WebDriver { get; set; }

        [JsonProperty("produrl")]
        public string produrl { get; set; }
        [JsonProperty("StagingUrl")]
        public string StagingUrl { get; set; }
        [JsonProperty("testurl")]
        public string testurl { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("DbSource")]
        public string DbSource { get; set; }
        
    }
}
