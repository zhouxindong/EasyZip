using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyZip.Base.Tests
{
    [TestClass()]
    public class BuiltInCompressTests
    {
        [TestMethod()]
        public void DeflateBytesTest()
        {
            //var data = new byte[10000000];
            //for (int i = 0; i < data.Length; i++)
            //{
            //    data[i] = (byte) i;
            //}

            byte[] data = new byte[10000000];

            var rnd = new Random();
            rnd.NextBytes(data);


            var deflated = BuiltInCompress.DeflateBytes(data);
            var inflated = BuiltInCompress.InflateBytes(deflated);
            Assert.AreEqual(inflated.Length, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], inflated[i]);
            }
        }

        [TestMethod()]
        public void DeflateStrTest()
        {
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var deflated = BuiltInCompress.DeflateStr(str);
            var inflated = BuiltInCompress.InflateStr(deflated);
            Assert.AreEqual(str, inflated);
        }

        [TestMethod()]
        public void GZipBytesTest()
        {
            byte[] data = new byte[10000000];

            var rnd = new Random();
            rnd.NextBytes(data);


            var deflated = BuiltInCompress.GZipBytes(data);
            var inflated = BuiltInCompress.UnGZipBytes(deflated);
            Assert.AreEqual(inflated.Length, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], inflated[i]);
            }
        }

        [TestMethod()]
        public void GZipStrTest()
        {
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var deflated = BuiltInCompress.GZipStr(str);
            var inflated = BuiltInCompress.UnGZipStr(deflated);
            Assert.AreEqual(str, inflated);
        }
    }
}