using System;
using System.IO;
using EasyZip.Base;
using EasyZip.Core;

namespace EasyZip.GZip
{
    /// <summary>
    /// An example class to demonstrate compression and decompression of GZip streams.
    /// </summary>
    public static class GZip
    {
        /// <summary>
        /// Decompress the <paramref name="inStream">input</paramref> writing 
        /// uncompressed data to the <paramref name="outStream">output stream</paramref>
        /// </summary>
        /// <param name="inStream">The readable stream containing data to decompress.</param>
        /// <param name="outStream">The output stream to receive the decompressed data.</param>
        /// <param name="isStreamOwner">Both streams are closed on completion if true.</param>
        /// <exception cref="ArgumentNullException">Input or output stream is null</exception>
        public static void Decompress(Stream inStream, Stream outStream, bool isStreamOwner)
        {
            if (inStream == null)
                throw new ArgumentNullException(nameof(inStream), "Input stream is null");

            if (outStream == null)
                throw new ArgumentNullException(nameof(outStream), "Output stream is null");

            try
            {
                using (GZipInputStream gzipInput = new GZipInputStream(inStream))
                {
                    gzipInput.IsStreamOwner = isStreamOwner;
                    Core.StreamUtils.Copy(gzipInput, outStream, new byte[4096]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // inStream is closed by the GZipInputStream if stream owner
                    outStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Compress the <paramref name="inStream">input stream</paramref> sending 
        /// result data to <paramref name="outStream">output stream</paramref>
        /// </summary>
        /// <param name="inStream">The readable stream to compress.</param>
        /// <param name="outStream">The output stream to receive the compressed data.</param>
        /// <param name="isStreamOwner">Both streams are closed on completion if true.</param>
        /// <param name="bufferSize">Deflate buffer size, minimum 512</param>
        /// <param name="level">Deflate compression level, 0-9</param>
        /// <exception cref="ArgumentNullException">Input or output stream is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Buffer Size is smaller than 512</exception>
        /// <exception cref="ArgumentOutOfRangeException">Compression level outside 0-9</exception>
        public static void Compress(Stream inStream, Stream outStream, bool isStreamOwner, int bufferSize = 512, int level = 6)
        {
            if (inStream == null)
                throw new ArgumentNullException(nameof(inStream), "Input stream is null");

            if (outStream == null)
                throw new ArgumentNullException(nameof(outStream), "Output stream is null");

            if (bufferSize < 512)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), "Deflate buffer size must be >= 512");

            if (level < Deflater.NO_COMPRESSION || level > Deflater.BEST_COMPRESSION)
                throw new ArgumentOutOfRangeException(nameof(level), "Compression level must be 0-9");

            try
            {
                using (GZipOutputStream gzipOutput = new GZipOutputStream(outStream, bufferSize))
                {
                    gzipOutput.SetLevel(level);
                    gzipOutput.IsStreamOwner = isStreamOwner;
                    Core.StreamUtils.Copy(inStream, gzipOutput, new byte[bufferSize]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // outStream is closed by the GZipOutputStream if stream owner
                    inStream.Dispose();
                }
            }
        }

        public static string CompressFile(string file_fullpath)
        {
            if (!File.Exists(file_fullpath))
                return null;
            var compressed_file = file_fullpath + ".gz";
            if (File.Exists(compressed_file))
                File.Delete(compressed_file);
            using (var s = new GZipOutputStream(File.Create(compressed_file)))
            using (var fs = File.OpenRead(file_fullpath))
            {
                byte[] buf = new byte[4096];
                StreamUtils.Copy(fs, s, buf);
            }
            return compressed_file;
        }

        public static byte[] DecompressFile(string file_fullpath)
        {
            if (!File.Exists(file_fullpath))
                return null;

            using (var s = new GZipInputStream(File.OpenRead(file_fullpath)))
            {
                using (var os = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    StreamUtils.Copy(s, os, buffer);
                    return os.ToArray();
                }
            }
        }
    }
}