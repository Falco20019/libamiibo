using LibAmiibo.Encryption;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace LibAmiibo
{
    public class Settings
    {
        private Settings() { }

        static Settings()
        {
            var basePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);


            var builder = new ConfigurationBuilder()
            .SetBasePath(basePath);

            if (File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                builder.AddJsonFile("appsettings.json");
            }
            else
            {
                builder.AddInMemoryCollection();
            }

            var config = builder.Build();

            AmiiboKeys = config["AmiiboKeys"] ?? Path.Combine(basePath, "key_retail.bin");
            CDNKeys = config["CDNKeys"] ?? Path.Combine(basePath, "cdn_keys.bin");
            TitleCacheDir = config["TitleCacheDir"] ?? Path.Combine(basePath, "titles");
        }

        private static string _AmiiboKeys = null;

        /// <summary>
        /// The full path to key_retail.bin
        /// </summary>
        public static string AmiiboKeys
        {
            get => _AmiiboKeys;
            set
            {
                _AmiiboKeys = value;

                if (value != null)
                {
                    Keys.AmiiboKeys = Encryption.AmiiboKeys.LoadKeys(value);
                }
            }
        }

        private static string _CDNKeys = null;

        /// <summary>
        /// The full path to cdn_keys.bin
        /// </summary>
        public static string CDNKeys
        {
            get => _CDNKeys;
            set
            {
                _CDNKeys = value;

                if (value != null)
                {
                    Keys.CDNKeys = Encryption.CDNKeys.LoadKeys(value);
                }
            }
        }

        public static string TitleCacheDir { get; set; }
    }
}
