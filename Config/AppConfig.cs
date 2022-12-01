using System;
using System.Collections.Generic;
using System.Text;

namespace selenium.xunit.framework.package.Config
{
    public static class AppConfig
    {
        public static string WebDriver { get; set; }

        public static string Environment { get; set; }
        public static string ProdUrl { get; set; }
        public static string TestUrl { get; set; }
        public static string StagingUrl { get; set; }
        public static string DbSource{get; set;}

        public static string BaseUrl()
        {
            var url = string.Empty;

            switch (Environment)
            {
                case "Prod":
                    url = ProdUrl;
                    break;
                case "test":
                    url = TestUrl;
                    break;
                case "staging":
                    url = StagingUrl;
                    break;                
                default:
                    break;
            }

            return url;
        }

        public static string SqlConnectionString
        {
            get
            {
                string connectionString;

                connectionString = $"Data Source={DbSource};Initial Catalog=;User ID=;Password=;";

                return connectionString;
            }
        }
    }
}
