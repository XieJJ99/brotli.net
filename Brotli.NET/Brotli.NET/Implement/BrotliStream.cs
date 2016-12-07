using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

using Brotli.Enum;
using Brotli.Exceptions;

namespace Brotli
{
    public class BrotliStream : Stream
    {
        public override bool CanRead
        {
            get
            {
                return mStream != null 
                    && mCompressionMode == CompressionMode.Decompress
                    && mStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get
            {
                return mStream != null
                    && mCompressionMode == CompressionMode.Compress
                    && mStream.CanWrite;
            }
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public BrotliStream(Stream baseStream, CompressionMode compressionMode)
        {
            mCompressionMode = compressionMode;
            mStream = baseStream;

            mPtrState = (mCompressionMode == CompressionMode.Compress)
                ? CreateEncoderInstance(DEFAULT_QUALITY, DEFAULT_WINDOW_SIZE)
                : CreateDecoderInstance();

            mPtrInputBuffer = Marshal.AllocHGlobal(BUFFER_SIZE);
            mPtrOutputBuffer = Marshal.AllocHGlobal(BUFFER_SIZE);
            mPtrNextInput = mPtrInputBuffer;
            mPtrNextOutput = mPtrOutputBuffer;

            mManagedBuffer = new Byte[BUFFER_SIZE];
        }

        /// <summary>
        /// Set the compress quality(0~11)
        /// </summary>
        /// <param name="quality">compress quality</param>
        public void SetQuality(uint quality)
        {
            if (quality > 11)
            {
                throw new ArgumentException(
                    "The Quality must be in the [0-11] range.",
                    nameof(quality));
            }

            Brolib.BrotliEncoderSetParameter(
                mPtrState,
                BrotliEncoderParameter.Quality,
                quality);
        }

        /// <summary>
        /// Set the compress LGWin(10~24)
        /// </summary>
        /// <param name="window">the window size</param>
        public void SetWindow(uint window)
        {
            if (window < 10 || window > 24)
            {
                throw new ArgumentException(
                    "The Window must be in the [10-24] range.",
                    nameof(window));
            }

            Brolib.BrotliEncoderSetParameter(
                mPtrState,
                BrotliEncoderParameter.LGWin,
                window);
        }

        public override void Flush()
        {
            if (mStream == null)
            {
                throw new ObjectDisposedException(
                    string.Empty,
                    "The underlying stream is null");
            }

            if (mCompressionMode == CompressionMode.Compress)
                FlushBrotliStream(false);
        }

        protected virtual void FlushBrotliStream(bool finished)
        {
            if (Brolib.BrotliEncoderIsFinished(mPtrState))
                return;

            BrotliEncoderOperation op = finished
                ? BrotliEncoderOperation.Finish
                : BrotliEncoderOperation.Flush;

            while (true)
            {
                UInt32 totalOut = 0;
                bool compressOk = Brolib.BrotliEncoderCompressStream(
                    mPtrState,
                    op,
                    ref mAvailableIn,
                    ref mPtrNextInput,
                    ref mAvailableOut,
                    ref mPtrNextOutput,
                    out totalOut);

                if (!compressOk)
                    throw new BrotliException("Unable to finish encode stream");

                bool extraData = mAvailableOut != BUFFER_SIZE;

                if (extraData)
                {
                    int bytesWrote = (int)(BUFFER_SIZE - mAvailableOut);
                    Marshal.Copy(mPtrOutputBuffer, mManagedBuffer, 0, bytesWrote);
                    mStream.Write(mManagedBuffer, 0, bytesWrote);
                    mAvailableOut = BUFFER_SIZE;
                    mPtrNextOutput = mPtrOutputBuffer;
                }

                if (Brolib.BrotliEncoderIsFinished(mPtrState) || !extraData)
                    break;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (mCompressionMode == CompressionMode.Compress)
                FlushBrotliStream(true);

            base.Dispose(disposing);

            mIntermediateStream.Dispose();

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

            mManagedBuffer = null;

            if (mCompressionMode == CompressionMode.Compress)
            {
                Brolib.BrotliEncoderDestroyInstance(mPtrState);
                return;
            }

            Brolib.BrotliDecoderDestroyInstance(mPtrState);
        }

        public void TruncateBeginning(MemoryStream ms, int numberOfBytesToRemove)
        {
            byte[] buffer = ms.GetBuffer();
            Buffer.BlockCopy(
                buffer,
                numberOfBytesToRemove,
                buffer,
                0,
                (int)ms.Length - numberOfBytesToRemove);

            ms.SetLength(ms.Length - numberOfBytesToRemove);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (mCompressionMode != CompressionMode.Decompress)
                throw new BrotliException("Can not read from the specified stream");

            int bytesRead = (int)(mIntermediateStream.Length - mReadOffset);
            bool endOfStream = false;
            bool errorDetected = false;

            while (bytesRead < count)
            {
                while (true)
                {
                    if (mLastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
                    {
                        mAvailableIn = (UInt32)mStream.Read(mManagedBuffer, 0, BUFFER_SIZE);
                        mPtrNextInput = mPtrInputBuffer;

                        if (mAvailableIn <= 0)
                        {
                            endOfStream = true;
                            break;
                        }

                        Marshal.Copy(mManagedBuffer, 0, mPtrInputBuffer, (int)mAvailableIn);
                    }
                    else if (mLastDecodeResult == BrotliDecoderResult.NeedsMoreOutput)
                    {
                        Marshal.Copy(mPtrOutputBuffer, mManagedBuffer, 0, BUFFER_SIZE);
                        mIntermediateStream.Write(mManagedBuffer, 0, BUFFER_SIZE);
                        bytesRead += BUFFER_SIZE;
                        mAvailableOut = BUFFER_SIZE;
                        mPtrNextOutput = mPtrOutputBuffer;
                    }
                    else
                    {
                        //Error or OK
                        endOfStream = true;
                        break;
                    }

                    uint totalCount = 0;
                    mLastDecodeResult = Brolib.BrotliDecoderDecompressStream(
                        mPtrState,
                        ref mAvailableIn,
                        ref mPtrNextInput,
                        ref mAvailableOut,
                        ref mPtrNextOutput,
                        out totalCount);

                    if (bytesRead >= count)
                        break;
                }

                if (endOfStream && !Brolib.BrotliDecoderIsFinished(mPtrState))
                {
                    errorDetected = true;
                }

                if (mLastDecodeResult == BrotliDecoderResult.Error || errorDetected)
                {
                    int error = Brolib.BrotliDecoderGetErrorCode(mPtrState);
                    string text = Brolib.BrotliDecoderErrorString(error);

                    throw new BrotliException(string.Format(
                        "Unable to decode stream,possibly corrupt data.Code={0}({1})",
                        error,
                        text));
                }

                if (endOfStream && !Brolib.BrotliDecoderIsFinished(mPtrState)
                    && mLastDecodeResult == BrotliDecoderResult.NeedsMoreInput)
                {
                    throw new BrotliException("Unable to decode stream,unexpected EOF");
                }

                if (endOfStream && mPtrNextOutput != mPtrOutputBuffer)
                {
                    int remainBytes = (int)(mPtrNextOutput.ToInt64() - mPtrOutputBuffer.ToInt64());
                    bytesRead += remainBytes;
                    Marshal.Copy(mPtrOutputBuffer, mManagedBuffer, 0, remainBytes);
                    mIntermediateStream.Write(mManagedBuffer, 0, remainBytes);
                    mPtrNextOutput = mPtrOutputBuffer;
                }
                if (endOfStream) break;
            }

            if (mIntermediateStream.Length - mReadOffset < count && !endOfStream)
                return 0;

            mIntermediateStream.Seek(mReadOffset, SeekOrigin.Begin);
            int bytesToRead = (int)(mIntermediateStream.Length - mReadOffset);
            if (bytesToRead > count) bytesToRead = count;
            mIntermediateStream.Read(buffer, offset, bytesToRead);
            TruncateBeginning(mIntermediateStream, mReadOffset + bytesToRead);
            mReadOffset = 0;

            return bytesToRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (mCompressionMode != CompressionMode.Compress)
                throw new BrotliException("Can not write to the specified stream");

            int remainingBytes = count;
            int currentOffset = offset;

            while (remainingBytes > 0)
            {
                int copyLen = remainingBytes > BUFFER_SIZE
                    ? BUFFER_SIZE
                    : remainingBytes;

                Marshal.Copy(buffer, currentOffset, mPtrInputBuffer, copyLen);
                remainingBytes -= copyLen;
                currentOffset += copyLen;
                mAvailableIn = (UInt32)copyLen;
                mPtrNextInput = mPtrInputBuffer;

                while (mAvailableIn > 0)
                {
                    uint totalOut = 0;

                    bool compressOk = Brolib.BrotliEncoderCompressStream(
                        mPtrState,
                        BrotliEncoderOperation.Process,
                        ref mAvailableIn,
                        ref mPtrNextInput,
                        ref mAvailableOut,
                        ref mPtrNextOutput,
                        out totalOut);

                    if (!compressOk)
                        throw new BrotliException("Unable to compress stream");

                    if (mAvailableOut == BUFFER_SIZE)
                        continue;

                    int bytesWrote = (int)(BUFFER_SIZE - mAvailableOut);
                    Marshal.Copy(mPtrOutputBuffer, mManagedBuffer, 0, bytesWrote);
                    mStream.Write(mManagedBuffer, 0, bytesWrote);
                    mAvailableOut = BUFFER_SIZE;
                    mPtrNextOutput = mPtrOutputBuffer;
                }

                if (Brolib.BrotliEncoderIsFinished(mPtrState))
                    break;
            }
        }

        static IntPtr CreateEncoderInstance(uint defaultQuality, uint defaultWindowSize)
        {
            IntPtr result = Brolib.BrotliEncoderCreateInstance();

            if (result == IntPtr.Zero)
                throw new BrotliException("Unable to create brotli encoder instance");

            Brolib.BrotliEncoderSetParameter(
                result,
                BrotliEncoderParameter.Quality,
                defaultQuality);

            Brolib.BrotliEncoderSetParameter(
                result,
                BrotliEncoderParameter.LGWin,
                defaultWindowSize);

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

        readonly Stream mStream;
        readonly MemoryStream mIntermediateStream = new MemoryStream();
        readonly CompressionMode mCompressionMode;
        readonly IntPtr mPtrState;

        IntPtr mPtrInputBuffer;
        IntPtr mPtrOutputBuffer;

        IntPtr mPtrNextInput;
        IntPtr mPtrNextOutput;
        UInt32 mAvailableIn;
        UInt32 mAvailableOut = BUFFER_SIZE;

        byte[] mManagedBuffer;

        int mReadOffset = 0;

        BrotliDecoderResult mLastDecodeResult = BrotliDecoderResult.NeedsMoreInput;
    }
}
