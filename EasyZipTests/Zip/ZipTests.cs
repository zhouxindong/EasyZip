using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyZip.Zip.Tests
{
    [TestClass()]
    public class ZipTests
    {
        [TestMethod()]
        public void CompressTest()
        {
            var file_name = @"c:\temp\doc\test.docx";
            Zip.Compress(file_name, @"c:\Code\test.zip");
        }
    }
}