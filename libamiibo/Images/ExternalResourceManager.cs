using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using StbSharp;

namespace LibAmiibo.Images
{
    class ExternalResourceManager
    {
        private Assembly Assembly { get; set; }
        private const string IMAGE_BASE = "LibAmiibo.Images.";

        public static readonly ExternalResourceManager Instance = new ExternalResourceManager();

        private ExternalResourceManager()
        {
            try
            {
                var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                var path = Path.Combine(assemblyPath, "libamiibo.images.dll");
                this.Assembly = Assembly.LoadFrom(path);
            }
            catch
            {
                // This happens if the image assembly is not found!
            }
        }

        public Image GetImage(string name)
        {
            var resFilestream = this.Assembly?.GetManifestResourceStream(IMAGE_BASE + name);
            if (resFilestream == null)
                return null;

            byte[] bytes = new byte[resFilestream.Length];
            resFilestream.Read(bytes, 0, bytes.Length);
            return StbImage.LoadFromMemory(bytes, StbImage.STBI_rgb_alpha);
        }

        public IEnumerable<string> GetNames()
        {
            return this.Assembly?.GetManifestResourceNames().Where(n => n.StartsWith(IMAGE_BASE)).Select(n => n.Substring(IMAGE_BASE.Length));
        }
    }
}
