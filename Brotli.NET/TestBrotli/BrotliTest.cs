using System;
using System.IO.Compression;
using System.Text;

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
            byte[] invalidInput = new byte[] { 0x1, 0x2, 0x3, 0x4 };
            byte[] output = new byte[64 * 1024];

            using (BrotliNET bc = new BrotliNET(CompressionMode.Decompress))
            {
                // No Assert.Throws(···) in Microsoft.VisualStudio.TestTools.UnitTesting.Assert ??
                AssertThrows(
                    () => { bc.Decompress(invalidInput, 0, 4, output); },
                    "No error was thrown for an invalid input.");
            }
        }

        [TestMethod]
        public void TestEncode()
        {
            byte[] input = Encoding.UTF8.GetBytes(TestResource.BingCN);
            byte[] output = new byte[input.Length];

            int outputLength = 0;
            using (BrotliNET bc = new BrotliNET(CompressionMode.Compress))
            {
                bc.Quality = 11;
                bc.Window = 22;

                outputLength = bc.Compress(input, 0, input.Length, output);
            }

            Assert.IsTrue(ArraySliceEquals(
                TestResource.BingCN_Compressed, 0, TestResource.BingCN_Compressed.Length,
                output, 0, outputLength),
                "The compressed file differs from the expected one.");
        }

        [TestMethod]
        public void TestDecode()
        {
            byte[] input = TestResource.BingCN_Compressed;
            byte[] output = new byte[input.Length * 4];

            int outputLength = 0;
            using (BrotliNET bc = new BrotliNET(CompressionMode.Decompress))
            {
                outputLength = bc.Decompress(input, 0, input.Length, output);
            }

            string textOutput = Encoding.UTF8.GetString(output, 0, outputLength);

            Assert.AreEqual(
                TestResource.BingCN, textOutput,
                "The decompressed file differs from the expected one.");
        }

        static bool ArraySliceEquals(
            byte[] left, int lStart, int lCount,
            byte[] right, int rStart, int rCount)
        {
            if (left == null || right == null)
                return ReferenceEquals(left, right);

            if (left.Length < lStart + lCount)
                return false;

            if (right.Length < rStart + rCount)
                return false;

            if (lCount != rCount)
                return false;

            for (int i = 0; i < lCount; i++)
            {
                if (left[lStart + i] != right[rStart + i])
                    return false;
            }

            return true;
        }

        static void AssertThrows(Action action, string message)
        {
            bool hasFailed = false;

            try
            {
                action();
            }
            catch (Exception)
            {
                hasFailed = true;
            }

            if (hasFailed)
                return;

            Assert.Fail(message);
        }
    }
}
