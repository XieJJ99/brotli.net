using System;
using System.IO.Compression;
using System.Runtime.InteropServices;

using Brotli.Enum;
using Brotli.Exceptions;

namespace Brotli
{
    public class BrotliNET : IDisposable
    {
        public BrotliNET(CompressionMode compressionMode)
        {
            mCompressionMode = compressionMode;

            mPtrState = (mCompressionMode == CompressionMode.Compress)
                ? CreateEncoderInstance(DEFAULT_QUALITY, DEFAULT_WINDOW_SIZE)
                : CreateDecoderInstance();

            mPtrInputBuffer = Marshal.AllocHGlobal(BUFFER_SIZE);
            mPtrOutputBuffer = Marshal.AllocHGlobal(BUFFER_SIZE);
            mPtrNextInput = mPtrInputBuffer;
            mPtrNextOutput = mPtrOutputBuffer;
        }

        public uint Quality
        {
            get { return mQuality; }
            set
            {
                AssertQualityInRange(value);
                mQuality = value;
                Brolib.BrotliEncoderSetParameter(
                    mPtrState,
                    BrotliEncoderParameter.Quality,
                    mQuality);
            }
        }

        public uint Window
        {
            get { return mWindowSize; }
            set
            {
                AssertWindowSizeInRange(value);
                mWindowSize = value;
                Brolib.BrotliEncoderSetParameter(
                    mPtrState,
                    BrotliEncoderParameter.LGWin,
                    mWindowSize);
            }
        }

        public int Compress(byte[] source, int offset, int count, byte[] destination)
        {
            if (mCompressionMode != CompressionMode.Compress)
            {
                throw new BrotliException(
                    "The BrotliStream instance can not compress the buffer.");
            }

            int destinationLength = 0;

            int remainingBytes = count;
            int currentOffset = offset;

            #region Process
            while (remainingBytes > 0)
            {
                int bytesToCopy = remainingBytes > BUFFER_SIZE
                    ? BUFFER_SIZE
                    : remainingBytes;

                Marshal.Copy(source, currentOffset, mPtrInputBuffer, bytesToCopy);
                remainingBytes -= bytesToCopy;
                currentOffset += bytesToCopy;
                mAvailableIn = (UInt32)bytesToCopy;
                mPtrNextInput = mPtrInputBuffer;

                while (mAvailableIn > 0)
                {
                    uint totalOut = 0;

                    bool processedOk = Brolib.BrotliEncoderCompressStream(
                        mPtrState,
                        BrotliEncoderOperation.Process,
                        ref mAvailableIn,
                        ref mPtrNextInput,
                        ref mAvailableOut,
                        ref mPtrNextOutput,
                        out totalOut);

                    if (!processedOk)
                        throw new BrotliException("Could not compress the buffer.");

                    if (mAvailableOut == BUFFER_SIZE)
                        continue;

                    int bytesProcessed = (int)(BUFFER_SIZE - mAvailableOut);
                    Marshal.Copy(
                        mPtrOutputBuffer, destination, destinationLength, bytesProcessed);
                    destinationLength += bytesProcessed;
                    mAvailableOut = BUFFER_SIZE;
                    mPtrNextOutput = mPtrOutputBuffer;
                }

                if (Brolib.BrotliEncoderIsFinished(mPtrState))
                    break;
            }
            #endregion

            #region Finish
            while (true)
            {
                UInt32 totalOut = 0;
                bool compressedOk = Brolib.BrotliEncoderCompressStream(
                    mPtrState,
                    BrotliEncoderOperation.Finish,
                    ref mAvailableIn,
                    ref mPtrNextInput,
                    ref mAvailableOut,
                    ref mPtrNextOutput,
                    out totalOut);

                if (!compressedOk)
                    throw new BrotliException("Unable to correctly compress the buffer");

                bool extraData = mAvailableOut != BUFFER_SIZE;

                if (extraData)
                {
                    int bytesWritten = (int)(BUFFER_SIZE - mAvailableOut);
                    Marshal.Copy(mPtrOutputBuffer, destination, destinationLength, bytesWritten);
                    destinationLength += bytesWritten;
                    mAvailableOut = BUFFER_SIZE;
                    mPtrNextOutput = mPtrOutputBuffer;
                }

                if (Brolib.BrotliEncoderIsFinished(mPtrState) || !extraData)
                    break;
            }
            #endregion

            return destinationLength;
        }

        public int Decompress(byte[] source, int offset, int count, byte[] destination)
        {
            if (mCompressionMode != CompressionMode.Decompress)
            {
                throw new BrotliException(
                    "The BrotliStream instance can not decompress the buffer.");
            }

            BrotliDecoderResult lastDecodeResult =
                BrotliDecoderResult.NeedsMoreInput;

            int bytesProcessed = 0;
            bool endOfSource = false;
            bool errorDetected = false;
            int destinationLength = 0;

            while (bytesProcessed < count)
            {
                while (true)
                {
                    if (lastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
                    {
                        mAvailableIn = (count - bytesProcessed) > BUFFER_SIZE
                            ? BUFFER_SIZE
                            : (uint)(count - bytesProcessed);

                        mPtrNextInput = mPtrInputBuffer;

                        if (mAvailableIn <= 0)
                        {
                            endOfSource = true;
                            break;
                        }

                        Marshal.Copy(
                            source,
                            offset + bytesProcessed,
                            mPtrInputBuffer,
                            (int)mAvailableIn);
                    }
                    else if (lastDecodeResult == BrotliDecoderResult.NeedsMoreOutput)
                    {
                        Marshal.Copy(
                            mPtrOutputBuffer,
                            destination,
                            destinationLength,
                            BUFFER_SIZE);

                        destinationLength += BUFFER_SIZE;
                        bytesProcessed += BUFFER_SIZE;
                        mAvailableOut = BUFFER_SIZE;
                        mPtrNextOutput = mPtrOutputBuffer;
                    }
                    else
                    {
                        // Error or OK
                        endOfSource = true;
                        break;
                    }

                    uint totalCount = 0;
                    lastDecodeResult = Brolib.BrotliDecoderDecompressStream(
                        mPtrState,
                        ref mAvailableIn,
                        ref mPtrNextInput,
                        ref mAvailableOut,
                        ref mPtrNextOutput,
                        out totalCount);

                    if (bytesProcessed >= destination.Length)
                        break;
                }

                if (endOfSource && !Brolib.BrotliDecoderIsFinished(mPtrState))
                {
                    errorDetected = true;
                }

                if (lastDecodeResult == BrotliDecoderResult.Error || errorDetected)
                {
                    int errorCode = Brolib.BrotliDecoderGetErrorCode(mPtrState);
                    string errorMessage = Brolib.BrotliDecoderErrorString(errorCode);

                    throw new BrotliException(string.Format(
                        "Unable to decompress the buffer. Error {0} - {1}",
                        errorCode, errorMessage));
                }

                if (endOfSource && Brolib.BrotliDecoderIsFinished(mPtrState)
                    && lastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
                {
                    throw new Exception("Unable to decompress the buffer. Unexpected EOF.");
                }

                if (endOfSource && mPtrNextOutput != mPtrOutputBuffer)
                {
                    int remainingBytes = (int)(mPtrNextOutput.ToInt64() - mPtrOutputBuffer.ToInt64());
                    bytesProcessed += remainingBytes;

                    if (destinationLength + remainingBytes > destination.Length)
                    {
                        throw new BrotliException(
                            "The destination buffer is smaller than the decompressed source buffer");
                    }

                    Marshal.Copy(mPtrOutputBuffer, destination, destinationLength, remainingBytes);
                    destinationLength += remainingBytes;
                    mPtrNextOutput = mPtrOutputBuffer;
                }

                if (endOfSource)
                    break;
            }

            return destinationLength;
        }

        public void Dispose()
        {
            if (mPtrInputBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(mPtrInputBuffer);
                mPtrInputBuffer = IntPtr.Zero;
            }

            if (mPtrOutputBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(mPtrOutputBuffer);
                mPtrOutputBuffer = IntPtr.Zero;
            }

            if (mCompressionMode == CompressionMode.Compress)
            {
                Brolib.BrotliEncoderDestroyInstance(mPtrState);
                return;
            }

            Brolib.BrotliDecoderDestroyInstance(mPtrState);
        }

        static void AssertWindowSizeInRange(uint windowSize)
        {
            if (windowSize >= 10 && windowSize <= 24)
                return;

            throw new ArgumentOutOfRangeException(
                "Window", "The window size must be between 10 and 24.");
        }

        static void AssertQualityInRange(uint quality)
        {
            if (quality <= 11)
                return;

            throw new ArgumentOutOfRangeException(
                "Quality", "The quality must be between 0 and 11.");
        }

        static IntPtr CreateEncoderInstance(uint quality, uint windowSize)
        {
            IntPtr result = Brolib.BrotliEncoderCreateInstance();

            if (result == IntPtr.Zero)
                throw new BrotliException("Unable to create brotli encoder instance");

            Brolib.BrotliEncoderSetParameter(
                result,
                BrotliEncoderParameter.Quality,
                quality);

            Brolib.BrotliEncoderSetParameter(
                result,
                BrotliEncoderParameter.LGWin,
                windowSize);

            return result;
        }

        static IntPtr CreateDecoderInstance()
        {
            IntPtr result = Brolib.BrotliDecoderCreateInstance();

            if (result == IntPtr.Zero)
                throw new BrotliException("Unable to create brotli decoder instance");

            return result;
        }

        const int BUFFER_SIZE = 64 * 1024;
        const uint DEFAULT_QUALITY = 5;
        const uint DEFAULT_WINDOW_SIZE = 22;

        readonly CompressionMode mCompressionMode;
        readonly IntPtr mPtrState;

        uint mQuality;
        uint mWindowSize;

        IntPtr mPtrInputBuffer;
        IntPtr mPtrNextInput;
        IntPtr mPtrOutputBuffer;
        IntPtr mPtrNextOutput;

        uint mAvailableIn;
        uint mAvailableOut = BUFFER_SIZE;
    }
}