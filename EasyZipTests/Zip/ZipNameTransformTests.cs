using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyZip.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyZip.Core;

namespace EasyZip.Zip.Tests
{
    [TestClass()]
    public class ZipNameTransformTests : TransformBase
    {
        [TestMethod()]
        public void Basic()
        {
            var t = new ZipNameTransform();

            TestFile(t, "abcdef", "abcdef");
            TestFile(t, @"\\uncpath\d1\file1", "file1");
            TestFile(t, @"C:\absolute\file2", "absolute/file2");

            // This is ignored but could be converted to 'file3'
            TestFile(t, @"./file3", "./file3");

            // The following relative paths cant be handled and are ignored
            TestFile(t, @"../file3", "../file3");
            TestFile(t, @".../file3", ".../file3");

            // Trick filenames.
            TestFile(t, @".....file3", ".....file3");
            TestFile(t, @"c::file", "_file");
        }

        [TestMethod]
        public void TooLong()
        {
            var zt = new ZipNameTransform();
            var veryLong = new string('x', 65536);
            try
            {
                zt.TransformDirectory(veryLong);
                Assert.Fail("Expected an exception");
            }
            catch (PathTooLongException)
            {
            }
        }

        [TestMethod]
        public void LengthBoundaryOk()
        {
            var zt = new ZipNameTransform();
            string veryLong = "c:\\" + new string('x', 65535);
            try
            {
                zt.TransformDirectory(veryLong);
            }
            catch
            {
                Assert.Fail("Expected no exception");
            }
        }

        [TestMethod]
        public void NameTransforms()
        {
            INameTransform t = new ZipNameTransform(@"C:\Slippery");
            Assert.AreEqual("Pongo/Directory/", t.TransformDirectory(@"C:\Slippery\Pongo\Directory"), "Value should be trimmed and converted");
            Assert.AreEqual("PoNgo/Directory/", t.TransformDirectory(@"c:\slipperY\PoNgo\Directory"), "Trimming should be case insensitive");
            Assert.AreEqual("slippery/Pongo/Directory/", t.TransformDirectory(@"d:\slippery\Pongo\Directory"), "Trimming should be case insensitive");

            Assert.AreEqual("Pongo/File", t.TransformFile(@"C:\Slippery\Pongo\File"), "Value should be trimmed and converted");
        }

        [TestMethod]
        public void FilenameCleaning()
        {
            Assert.AreEqual(0, string.Compare(ZipEntry.CleanName("hello"), "hello", StringComparison.Ordinal));
            Assert.AreEqual(0, string.Compare(ZipEntry.CleanName(@"z:\eccles"), "eccles", StringComparison.Ordinal));
            Assert.AreEqual(0, string.Compare(ZipEntry.CleanName(@"\\server\share\eccles"), "eccles", StringComparison.Ordinal));
            Assert.AreEqual(0, string.Compare(ZipEntry.CleanName(@"\\server\share\dir\eccles"), "dir/eccles", StringComparison.Ordinal));
        }

        [TestMethod]
        public void PathalogicalNames()
        {
            string badName = ".*:\\zy3$";

            Assert.IsFalse(ZipNameTransform.IsValidName(badName));

            var t = new ZipNameTransform();
            string result = t.TransformFile(badName);

            Assert.IsTrue(ZipNameTransform.IsValidName(result));
        }
    }
}