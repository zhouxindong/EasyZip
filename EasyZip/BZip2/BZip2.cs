using System;
using System.IO;
using EasyZip.Core;

namespace EasyZip.BZip2
{
    /// <summary>
    /// An example class to demonstrate compression and decompression of BZip2 streams.
    /// </summary>
    public static class BZip2
    {
        /// <summary>
        /// Decompress the <paramref name="inStream">input</paramref> writing 
        /// uncompressed data to the <paramref name="outStream">output stream</paramref>
        /// </summary>
        /// <param name="inStream">The readable stream containing data to decompress.</param>
        /// <param name="outStream">The output stream to receive the decompressed data.</param>
        /// <param name="isStreamOwner">Both streams are closed on completion if true.</param>
        public static void Decompress(Stream inStream, Stream outStream, bool isStreamOwner)
        {
            if (inStream == null || outStream == null)
            {
                throw new Exception("Null Stream");
            }

            try
            {
                using (BZip2InputStream bzipInput = new BZip2InputStream(inStream))
                {
                    bzipInput.IsStreamOwner = isStreamOwner;
                    StreamUtils.Copy(bzipInput, outStream, new byte[4096]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // inStream is closed by the BZip2InputStream if stream owner
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
        /// <param name="level">Block size acts as compression level (1 to 9) with 1 giving 
        /// the lowest compression and 9 the highest.</param>
        public static void Compress(Stream inStream, Stream outStream, bool isStreamOwner, int level)
        {
            if (inStream == null || outStream == null)
            {
                throw new Exception("Null Stream");
            }

            try
            {
                using (BZip2OutputStream bzipOutput = new BZip2OutputStream(outStream, level))
                {
                    bzipOutput.IsStreamOwner = isStreamOwner;
                    StreamUtils.Copy(inStream, bzipOutput, new byte[4096]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // outStream is closed by the BZip2OutputStream if stream owner
                    inStream.Dispose();
                }
            }
        }

        public static byte[] Compress(byte[] data)
        {
            var input_stream = new MemoryStream(data);
            var output_stream = new MemoryStream();
            Compress(input_stream, output_stream, true, 9);
            return output_stream.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            var input_stream = new MemoryStream(data);
            var decompressed_stream = new MemoryStream();
            Decompress(input_stream, decompressed_stream, true);
            return decompressed_stream.ToArray();
        }
    }

}