using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace BackManager.Utility.Helper
{
    public class ConfigurationHelper<T> where T : class, new()
    {
        //索引器必须以this关键字定义，其实这个this就是类实例化之后的对象
        public T this[string index] => ConfigurationManager.GetAppSettings<T>(index);
    }
    public static T GetAppSettings<T>(string key) where T : class, new()
        {
            return GetAppSettings<T>(key, "appsettings.json");
        }
        public static T GetAppSettings<T>(string key,string Path) where T : class, new()
        {
            string baseDir = AppContext.BaseDirectory;
            int indexBin = baseDir.IndexOf("bin");
            // log.Write(baseDir);
            string subToSrc = "";

            if (indexBin > 0)
            {
                subToSrc = baseDir.Substring(0, indexBin);
            }
            else
            {
                subToSrc = baseDir;
            }

            string currentClassDir = subToSrc;

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(currentClassDir)
                .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
                .Build();

            T appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }
}
