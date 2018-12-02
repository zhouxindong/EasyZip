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
            byte[] buffer = new byte[1000000];

            var rnd = new Random();
            rnd.NextBytes(buffer);


            var deflated = Deflater.Do(buffer);
            var inflated = Inflater.Do(deflated);

            Assert.AreEqual(buffer.Length, inflated.Length);

            for (var i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(buffer[i], inflated[i]);
            }
        }

        [TestMethod]
        public void CloseDeflatorWithNestedUsing()
        {
            string tempFile = null;
            try
            {
                tempFile = Path.GetTempPath();
            }
            catch (SecurityException)
            {
            }

            Assert.IsNotNull(tempFile, "No permission to execute this test?");

            tempFile = Path.Combine(tempFile, "SharpZipTest.Zip");
            using (FileStream diskFile = File.Create(tempFile))
            using (DeflaterOutputStream deflator = new DeflaterOutputStream(diskFile))
            using (StreamWriter textWriter = new StreamWriter(deflator))
            {
                textWriter.Write("Hello");
                textWriter.Flush();
            }

            using (FileStream diskFile = File.OpenRead(tempFile))
            using (InflaterInputStream deflator = new InflaterInputStream(diskFile))
            using (StreamReader textReader = new StreamReader(deflator))
            {
                char[] buffer = new char[5];
                int readCount = textReader.Read(buffer, 0, 5);
                Assert.AreEqual(5, readCount);

                var b = new StringBuilder();
                b.Append(buffer);
                Assert.AreEqual("Hello", b.ToString());

            }

            File.Delete(tempFile);

        }

    }
}