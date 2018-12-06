using System.IO;
using EasyZip.Core;

namespace EasyZip.Zip
{
    public class Zip
    {
        public static void CompressFile(string file_name, string zip_name)
        {
            byte[] buf = new byte[4096];
            using (var out_stream = new ZipOutputStream(File.Create(zip_name)))
            {
                out_stream.SetLevel(9);
                var entry = new ZipEntry(file_name);
                out_stream.PutNextEntry(entry);
                using (var fs = File.OpenRead(file_name))
                {
                    StreamUtils.Copy(fs, out_stream, buf);
                }
            }
        }
    }
}