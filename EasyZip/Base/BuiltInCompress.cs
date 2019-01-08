using System.IO;
using System.IO.Compression;
using System.Text;

namespace EasyZip.Base
{
    public class BuiltInCompress
    {
        #region Defalate

        public static byte[] DeflateStr(string str)
        {
            return DeflateBytes(Encoding.UTF8.GetBytes(str));
        }

        public static byte[] DeflateBytes(byte[] data)
        {
            //var out_ms = new MemoryStream();
            //var deflate = new DeflateStream(out_ms, CompressionMode.Compress);
            //deflate.Write(buf, 0, buf.Length);
            //deflate.Dispose();
            //return out_ms.ToArray();
            var ms = new MemoryStream(data) {Position = 0};
            var outms = new MemoryStream();
            using (var deflate_stream = new DeflateStream(outms, CompressionMode.Compress, true))
            {
                var buf = new byte[4096];
                int len;
                while ((len = ms.Read(buf, 0, buf.Length)) > 0)
                    deflate_stream.Write(buf, 0, len);
            }
            return outms.ToArray();
        }

        public static string InflateStr(byte[] data)
        {
            return Encoding.UTF8.GetString(InflateBytes(data));
        }

        public static byte[] InflateBytes(byte[] data)
        {
            var ms = new MemoryStream(data) {Position = 0};
            var outms = new MemoryStream();
            using (var deflate_stream = new DeflateStream(ms, CompressionMode.Decompress, true))
            {
                var tmp_buf = new byte[4096];
                int len;
                while ((len = deflate_stream.Read(tmp_buf, 0, tmp_buf.Length)) > 0)
                    outms.Write(tmp_buf, 0, len);
            }
            return outms.ToArray();
        }

        #endregion

        #region GZip

        public static byte[] GZipStr(string str)
        {
            return GZipBytes(Encoding.UTF8.GetBytes(str));
        }

        public static byte[] GZipBytes(byte[] data)
        {
            var ms = new MemoryStream(data) { Position = 0 };
            var outms = new MemoryStream();
            using (var deflate_stream = new GZipStream(outms, CompressionMode.Compress, true))
            {
                var buf = new byte[4096];
                int len;
                while ((len = ms.Read(buf, 0, buf.Length)) > 0)
                    deflate_stream.Write(buf, 0, len);
            }
            return outms.ToArray();
        }

        public static string UnGZipStr(byte[] data)
        {
            return Encoding.UTF8.GetString(UnGZipBytes(data));
        }

        public static byte[] UnGZipBytes(byte[] data)
        {
            var ms = new MemoryStream(data) { Position = 0 };
            var outms = new MemoryStream();
            using (var deflate_stream = new GZipStream(ms, CompressionMode.Decompress, true))
            {
                var tmp_buf = new byte[4096];
                int len;
                while ((len = deflate_stream.Read(tmp_buf, 0, tmp_buf.Length)) > 0)
                    outms.Write(tmp_buf, 0, len);
            }
            return outms.ToArray();
        }


        #endregion
    }
}