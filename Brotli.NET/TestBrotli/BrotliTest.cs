using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Brotli;
using Brotli.Exceptions;

namespace TestBrotli
{
    [TestClass]
    public class BrotliTest
    {
        [TestMethod]
        public void TestErrorDetection()
        {
            Boolean errorDetected = false;
            using (System.IO.MemoryStream msInvalid = new System.IO.MemoryStream())
            {
                var rawBytes = new Byte[] { 0x1, 0x2, 0x3, 0x4 };
                msInvalid.Write(rawBytes, 0, rawBytes.Length);
                msInvalid.Seek(0, System.IO.SeekOrigin.Begin);

                using (BrotliStream bs = new BrotliStream(
                    msInvalid, System.IO.Compression.CompressionMode.Decompress))
                using (System.IO.MemoryStream msOut = new System.IO.MemoryStream())
                {
                    int bufferSize = 64 * 1024;
                    byte[] buffer = new byte[bufferSize];
                    while (true)
                    {
                        try
                        {
                            var cnt = bs.Read(buffer, 0, bufferSize);
                            if (cnt <= 0) break;
                            msOut.Write(buffer, 0, cnt);
                        }
                        catch (BrotliException)
                        {
                            errorDetected = true;
                            break;
                        }
                    }
                }
            }

            Assert.IsTrue(errorDetected, "No error was detected!");
        }


        public Boolean ArrayEqual(Byte[] a1, Byte[] a2)
        {
            if (a1 == null && a2 == null) return true;
            if (a1 == null || a2 == null) return false;
            if (a1.Length != a2.Length) return false;
            for (var i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i]) return false;
            }
            return true;
        }

        [TestMethod]
        public void TestEncode()
        {
            var input = System.Text.Encoding.UTF8.GetBytes(TestResource.BingCN);
            Byte[] output = null;
            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
            using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
            {
                using (BrotliStream bs = new BrotliStream(
                    msOutput, System.IO.Compression.CompressionMode.Compress))
                {
                    bs.SetQuality(11);
                    bs.SetWindow(22);
                    msInput.CopyTo(bs);
                }

                output = msOutput.ToArray();

                Assert.IsTrue(
                    ArrayEqual(output, TestResource.BingCN_Compressed),
                    "The compressed file differs from the expected one");
            }
        }

        [TestMethod]
        public void TestDecode()
        {
            var input = TestResource.BingCN_Compressed;

            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
            using (BrotliStream bs = new BrotliStream(msInput, System.IO.Compression.CompressionMode.Decompress))
            using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
            {
                bs.CopyTo(msOutput);

                string text = System.Text.Encoding.UTF8.GetString(msOutput.ToArray());

                Assert.AreEqual(
                    text, TestResource.BingCN,
                    "The uncompressed file differs from the expected one.");
            }
        }
    }
}
