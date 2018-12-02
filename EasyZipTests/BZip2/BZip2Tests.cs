using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.BZip2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyZip.Base;
using EasyZipTests;

namespace EasyZip.BZip2.Tests
{
    [TestClass()]
    public class BZip2Tests
    {
        [TestMethod()]
        public void BasicRoundTrip()
        {
            var ms = new MemoryStream();
            var outStream = new BZip2OutputStream(ms);

            byte[] buf = new byte[10000];
            var rnd = new Random();
            rnd.NextBytes(buf);

            outStream.Write(buf, 0, buf.Length);
            outStream.Close();
            ms = new MemoryStream(/*ms.GetBuffer()*/ms.ToArray());
            ms.Seek(0, SeekOrigin.Begin);

            using (BZip2InputStream inStream = new BZip2InputStream(ms))
            {
                byte[] buf2 = new byte[buf.Length];
                int pos = 0;
                while (true)
                {
                    int numRead = inStream.Read(buf2, pos, 4096);
                    if (numRead <= 0)
                    {
                        break;
                    }
                    pos += numRead;
                }

                for (int i = 0; i < buf.Length; ++i)
                {
                    Assert.AreEqual(buf2[i], buf[i]);
                }
            }
        }

        [TestMethod()]
        public void CompressTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                byte[] buf = new byte[10000];
                var rnd = new Random();
                rnd.NextBytes(buf);

                var input_stream = new MemoryStream(buf);
                var output_stream = new MemoryStream();
                BZip2.Compress(input_stream, output_stream, true, 9);

                input_stream = new MemoryStream(output_stream.ToArray());
                var decompressed_stream = new MemoryStream();
                BZip2.Decompress(input_stream, decompressed_stream, true);

                var decompressed_bytes = decompressed_stream.ToArray();
                Assert.AreEqual(buf.Length, decompressed_bytes.Length);

                //for (int i = 0; i < buf.Length; i++)
                //{
                //    Assert.AreEqual(buf[i], decompressed_bytes[i]);
                //}
                Thread.Sleep(1);
                Console.WriteLine(MemInfo.GetCurProcessMem());
            }
        }

        [TestMethod()]
        public void CompressTest1()
        {
            for (int i = 0; i < 1000; i++)
            {

                byte[] buffer = new byte[1000000];

                var rnd = new Random();
                rnd.NextBytes(buffer);


                var deflated = BZip2.Compress(buffer);
                var inflated = BZip2.Decompress(deflated);

                Assert.AreEqual(buffer.Length, inflated.Length);

                //for (var i = 0; i < buffer.Length; i++)
                //{
                //    Assert.AreEqual(buffer[i], inflated[i]);
                //}

                if (i%100 == 0)
                    Console.WriteLine(MemInfo.GetCurProcessMem());

            }
        }
    }
}