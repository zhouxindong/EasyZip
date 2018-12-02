using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using EasyZip.Base.Streams;
using EasyZipTests;

namespace EasyZip.Base.Tests
{
    [TestClass()]
    public class DeflaterTests
    {
        [TestMethod()]
        public void DoTest()
        {
            byte[] buffer = new byte[100000];

            var rnd = new Random();
            rnd.NextBytes(buffer);

            double ratio;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var output = Deflater.Do(buffer);
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            Console.WriteLine($"origin {buffer.Length} bytes, deflated {output.Length} bytes.");

            watch.Reset();
            watch.Start();
            Deflater.Do(buffer);
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            Console.WriteLine($"origin {buffer.Length} bytes, deflated {output.Length} bytes.");
        }

        [TestMethod()]
        public void DeflateInflateTest()
        {
            for (int i = 0; i < 10; i++)
            {
                byte[] buffer = new byte[100000];

                //var rnd = new Random();
                //rnd.NextBytes(buffer);


                var deflated = Deflater.Do(buffer);
                var inflated = Inflater.Do(deflated);
                double ratio = (double)deflated.Length/buffer.Length;
                Assert.AreEqual(buffer.Length, inflated.Length);

                var deflated_bzip = BZip2.BZip2.Compress(buffer);
                var inflated_bzip = BZip2.BZip2.Decompress(deflated_bzip);
                double ratio2 = (double) deflated_bzip.Length/buffer.Length;

                Console.WriteLine($"Base: {ratio}, BZip2: {ratio2}");
            }
        }

    }
}