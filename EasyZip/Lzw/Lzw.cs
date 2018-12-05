using System;
using System.IO;
using EasyZip.Core;

namespace EasyZip.Lzw
{
    public class Lzw
    {
        public static void DecompressFile(string file_fullname)
        {
            if (!Path.HasExtension(file_fullname) || Path.GetExtension(file_fullname) != ".Z" ||
                Path.GetExtension(file_fullname) != ".z")
                throw new InvalidOperationException("file extension can't recognised");
            if (!File.Exists(file_fullname))
                return;

            using (var in_stream = new LzwInputStream(File.OpenRead(file_fullname)))
            using (
                var out_file =
                    File.Create(Path.Combine(Path.GetDirectoryName(file_fullname),
                        Path.GetFileNameWithoutExtension(file_fullname))))
            {
                var buffer = new byte[4096];
                StreamUtils.Copy(in_stream, out_file, buffer);
            }
        }
    }
}