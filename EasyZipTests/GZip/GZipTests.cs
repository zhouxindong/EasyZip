using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyZip.Core;
using EasyZipTests;

namespace EasyZip.GZip.Tests
{
    [TestClass()]
    public class GZipTests
    {
        [TestMethod]
        public void TestGZip()
        {
            var ms = new MemoryStream();
            var outStream = new GZipOutputStream(ms);

            byte[] buf = new byte[100000];
            var rnd = new Random();
            rnd.NextBytes(buf);

            outStream.Write(buf, 0, buf.Length);
            outStream.Flush();
            outStream.Finish();

            ms.Seek(0, SeekOrigin.Begin);

            var inStream = new GZipInputStream(ms);
            byte[] buf2 = new byte[buf.Length];
            int currentIndex = 0;
            int count = buf2.Length;

            while (true)
            {
                int numRead = inStream.Read(buf2, currentIndex, count);
                if (numRead <= 0)
                {
                    break;
                }
                currentIndex += numRead;
                count -= numRead;
            }

            Assert.AreEqual(0, count);

            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.AreEqual(buf2[i], buf[i]);
            }
        }

        /// <summary>
        /// Writing GZip headers is delayed so that this stream can be used with HTTP/IIS.
        /// </summary>
        [TestMethod]
        public void DelayedHeaderWriteNoData()
        {
            var ms = new MemoryStream();
            Assert.AreEqual(0, ms.Length);

            using (GZipOutputStream outStream = new GZipOutputStream(ms))
            {
                Assert.AreEqual(0, ms.Length);
            }

            byte[] data = ms.ToArray();

            Assert.IsTrue(data.Length > 0);
        }

        [TestMethod]
        public void DelayedHeaderWriteWithData()
        {
            var ms = new MemoryStream();
            Assert.AreEqual(0, ms.Length);
            using (GZipOutputStream outStream = new GZipOutputStream(ms))
            {
                Assert.AreEqual(0, ms.Length);
                outStream.WriteByte(45);

                // Should in fact contain header right now with
                // 1 byte in the compression pipeline
                Assert.AreEqual(10, ms.Length);
            }
            byte[] data = ms.ToArray();

            Assert.IsTrue(data.Length > 0);
        }

        [TestMethod]
        public void ZeroLengthInputStream()
        {
            var gzi = new GZipInputStream(new MemoryStream());
            bool exception = false;
            int retval = int.MinValue;
            try
            {
                retval = gzi.ReadByte();
            }
            catch
            {
                exception = true;
            }

            Assert.IsFalse(exception, "reading from an empty stream should not cause an exception");
            Assert.AreEqual(retval, -1);
        }

        [TestMethod]
        public void TrailingGarbage()
        {
            /* ARRANGE */
            var ms = new MemoryStream();
            var outStream = new GZipOutputStream(ms);

            // input buffer to be compressed
            byte[] buf = new byte[100000];
            var rnd = new Random();
            rnd.NextBytes(buf);

            // compress input buffer
            outStream.Write(buf, 0, buf.Length);
            outStream.Flush();
            outStream.Finish();

            // generate random trailing garbage and add to the compressed stream
            byte[] garbage = new byte[4096];
            rnd.NextBytes(garbage);
            ms.Write(garbage, 0, garbage.Length);

            // rewind the concatenated stream
            ms.Seek(0, SeekOrigin.Begin);


            /* ACT */
            // decompress concatenated stream
            var inStream = new GZipInputStream(ms);
            byte[] buf2 = new byte[buf.Length];
            int currentIndex = 0;
            int count = buf2.Length;
            while (true)
            {
                int numRead = inStream.Read(buf2, currentIndex, count);
                if (numRead <= 0)
                {
                    break;
                }
                currentIndex += numRead;
                count -= numRead;
            }


            /* ASSERT */
            Assert.AreEqual(0, count);
            for (int i = 0; i < buf.Length; ++i)
            {
                Assert.AreEqual(buf2[i], buf[i]);
            }
        }

        private string file_name = @"C:\temp\test.docx";

        [TestMethod]
        public void FileCompressTest()
        {
            var origin_data = File.ReadAllBytes(file_name);
            var compressed_file = GZip.CompressFile(file_name);
            var buf = GZip.DecompressFile(compressed_file);
            Assert.AreEqual(origin_data.Length, buf.Length);
            for (int i = 0; i < origin_data.Length; i++)
            {
                Assert.AreEqual(origin_data[i], buf[i]);
            }
        }

        [TestMethod]
        public void FileDecompressTest()
        {
            using (Stream inStream = new GZipInputStream(File.OpenRead(file_name + ".gz")))
            {
                using (FileStream outStream = File.Create(@"C:\temp\test_de.docx"))
                {
                    byte[] buffer = new byte[4096];
                    StreamUtils.Copy(inStream, outStream, buffer);
                }
            }
        }
    }
}