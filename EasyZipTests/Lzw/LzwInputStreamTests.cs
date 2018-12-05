using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.Lzw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyZip.Lzw.Tests
{
    [TestClass()]
    public class LzwInputStreamTests
    {
        [TestMethod]
        public void ZeroLengthInputStream()
        {
            var lis = new LzwInputStream(new MemoryStream());
            bool exception = false;
            try
            {
                lis.ReadByte();
            }
            catch
            {
                exception = true;
            }

            Assert.IsTrue(exception, "reading from an empty stream should cause an exception");
        }
    }
}