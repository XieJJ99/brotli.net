using System;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brotli;

namespace TestBrotli
{
    [TestClass]
    public class BrotliTest
    {
        [TestMethod]
        public void TestErrorDetection()
        {
            Boolean errorDetected = false;
            var errorCode = 0;
            using (System.IO.MemoryStream msInvalid = new System.IO.MemoryStream())
            {
                var rawBytes = new Byte[] { 0x1, 0x2, 0x3, 0x4 };
                msInvalid.Write(rawBytes, 0, rawBytes.Length);
                msInvalid.Seek(0, System.IO.SeekOrigin.Begin);

                using (BrotliStream bs = new BrotliStream(msInvalid, System.IO.Compression.CompressionMode.Decompress))
                using (System.IO.MemoryStream msOut = new System.IO.MemoryStream())
                {
                    int bufferSize = 64 * 1024;
                    Byte[] buffer = new Byte[bufferSize];
                    while (true)
                    {
                        try
                        {
                            var cnt = bs.Read(buffer, 0, bufferSize);
                            if (cnt <= 0) break;
                            msOut.Write(buffer, 0, cnt);
                        }
                        catch (BrotliDecodeException bde )
                        {
                            errorDetected = true;
                            errorCode = bde.Code;
                            break;
                        }
                    }
                    //System.IO.File.WriteAllBytes(@"C:\Temp\MSN20160606_original.pdf", msOut.ToArray());
                }
            }
            Assert.IsTrue(errorDetected);
            Assert.AreEqual(2, errorCode);
        }



        public Boolean ArrayEqual(Byte[] a1,Byte[] a2)
        {
            if (a1 == null && a2 == null) return true;
            if (a1 == null || a2 == null) return false;
            if (a1.Length != a2.Length) return false;
            for (var i=0;i<a1.Length;i++)
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
            using (BrotliStream bs = new BrotliStream(msOutput, System.IO.Compression.CompressionMode.Compress))
            {
                bs.SetQuality(11);
                bs.SetWindow(22);
                msInput.CopyTo(bs);
                bs.Close();
                output = msOutput.ToArray();
                Boolean eq = ArrayEqual(output, TestResource.BingCN_Compressed);
                Assert.AreEqual(true, eq);

            }
        }

        [TestMethod]
        public void TestDecode()
        {
            var input = TestResource.BingCN_Compressed;
            Byte[] output = null;
            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
            using (BrotliStream bs = new BrotliStream(msInput, System.IO.Compression.CompressionMode.Decompress))
            using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
            {
                bs.CopyTo(msOutput);
                msOutput.Seek(0, System.IO.SeekOrigin.Begin);
                output = msOutput.ToArray();
                String text = System.Text.Encoding.UTF8.GetString(output);
                Assert.AreEqual(text, TestResource.BingCN);

            }

        }

	    [TestMethod]
	    public void EmptyStream()
	    {
		    var memoryStream = new MemoryStream();
		    var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true);
		    brotliStream.Flush();
			brotliStream.Dispose();
		    Assert.AreEqual(0, memoryStream.Length);
	    }
	}
}
