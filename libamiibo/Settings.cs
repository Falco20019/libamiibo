using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace LibAmiibo
{
    class Settings
    {
        private Settings() { }

        static Settings()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath))
            .AddJsonFile("appsettings.json");

            Default = builder.Build();
        }

        public static IConfigurationRoot Default { get; }
    }
}
