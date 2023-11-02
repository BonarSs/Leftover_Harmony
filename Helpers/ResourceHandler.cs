using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leftover_Harmony.Helpers
{
    public static class ResourceHandler
    {
        public static byte[] GetResource(string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null) throw new FileNotFoundException($"The resource specified by path '{path}' was not found.");
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
