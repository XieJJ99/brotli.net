using System;
using Brotli;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FXTestBrotli
{
    [TestClass]
    public class FX45BroltliTest
    {
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
        public void TestCreateInstance()
        {
            var input = new byte[] { 1, 2, 3, 4, 1, 2, 3, 4 };
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
                Boolean eq = ArrayEqual(output, new byte[] { 0x8b,0x03,0x80,1, 2, 3, 4,1,2,3,4,0x3 });
                Assert.IsTrue(eq);

            }

        }
    }
}
