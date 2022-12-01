using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace selenium.xunit.framework.package.Config
{
    public class ConfigReader
    {
        public static void SetAppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configurationRoot = builder.Build();

            AppConfig.WebDriver = configurationRoot.GetSection("appSettings").Get<AppSettings>().WebDriver;
            
            AppConfig.Environment = configurationRoot.GetSection("appSettings").Get<AppSettings>().Environment;
            AppConfig.ProdUrl = configurationRoot.GetSection("appSettings").Get<AppSettings>().produrl;
            AppConfig.StagingUrl = configurationRoot.GetSection("appSettings").Get<AppSettings>().StagingUrl;
            AppConfig.TestUrl = configurationRoot.GetSection("appSettings").Get<AppSettings>().testurl;
            
            AppConfig.DbSource = configurationRoot.GetSection("appSettings").Get<AppSettings>().DbSource;
           
        }
    }
}
