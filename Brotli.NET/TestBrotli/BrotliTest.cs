using System;
using Xunit;
using Brotli;
using System.IO;
using System.IO.Compression;
using BrotliStream = Brotli.BrotliStream;

namespace TestBrotli
{
    public class BrotliTest
    {
        //Use resource stream directly since .net core won't support windows.forms until v3
        public static byte[] GetBytes(string name)
        {
            var assembly = typeof(BrotliTest).Assembly;
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new Exception(
                        $"Resource {name} not found in {assembly.FullName}.  Valid resources are: {string.Join(", ", assembly.GetManifestResourceNames())}.");
                }
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        [Fact]
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
            Assert.True(errorDetected);
            Assert.Equal(2, errorCode);
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

        [Fact]
        public void TestEmptyStream()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true))
                {
                    brotliStream.Flush();
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                var data = memoryStream.ToArray();
                using (var bs = new BrotliStream(memoryStream, CompressionMode.Decompress))
                using (var msOutput = new MemoryStream())
                {
                    bs.CopyTo(msOutput); // goes bang
                    msOutput.Seek(0, SeekOrigin.Begin);
                    var output = msOutput.ToArray();
                    Assert.True(output.Length == 0);
                }
            }
        }

        [Fact]
        public void TestEncode()
        {
            var input = GetBytes("TestBrotli.Resource.BingCN.bin");
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
                Boolean eq = ArrayEqual(output, GetBytes("TestBrotli.Resource.BingCN_Compressed.bin"));
                Assert.True(eq);

            }
        }

        [Fact]
        public void TestDecode()
        {
            var input = GetBytes("TestBrotli.Resource.BingCN_Compressed.bin");
            Byte[] output = null;
            using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
            using (BrotliStream bs = new BrotliStream(msInput, System.IO.Compression.CompressionMode.Decompress))
            using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
            {
                bs.CopyTo(msOutput);
                msOutput.Seek(0, System.IO.SeekOrigin.Begin);
                output = msOutput.ToArray();                
                Assert.True(ArrayEqual(output, GetBytes("TestBrotli.Resource.BingCN.bin") ));

            }

        }
    }
}
