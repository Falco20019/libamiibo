using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace LibAmiibo.Images
{
    class ExternalResourceManager
    {
        public ResourceManager ResourceManager { get; private set; }

        public static readonly ExternalResourceManager Instance = new ExternalResourceManager();

        private ExternalResourceManager()
        {
            try
            {
                var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                var path = Path.Combine(assemblyPath, "libamiibo.images.dll");
                Assembly imageAssembly = Assembly.LoadFrom(path);
                this.ResourceManager = new ResourceManager("LibAmiibo.Images.Resources", imageAssembly);
            }
            catch
            {
                // This happens if the image assembly is not found!
            }
        }
    }
}
