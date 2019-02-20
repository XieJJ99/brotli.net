using System;
using System.Collections.Generic;
using System.Text;

namespace Brotli
{
    internal class Delegate32
    {
        #region Encoder
        internal delegate IntPtr BrotliEncoderCreateInstanceDelegate(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        internal delegate bool BrotliEncoderSetParameterDelegate(IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

        //delegate void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

        internal delegate bool BrotliEncoderCompressStreamDelegate(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        internal delegate bool BrotliEncoderIsFinishedDelegate(IntPtr state);

        internal delegate void BrotliEncoderDestroyInstanceDelegate(IntPtr state);
        internal delegate UInt32 BrotliEncoderVersionDelegate();
        internal delegate IntPtr BrotliEncoderTakeOutputDelegate(IntPtr state, ref UInt32 size);
        #endregion
        #region Decoder
        internal delegate IntPtr BrotliDecoderCreateInstanceDelegate(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        internal delegate bool BrotliDecoderSetParameter(IntPtr state, BrotliDecoderParameter param, UInt32 value);
        //delegate void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

        internal delegate BrotliDecoderResult BrotliDecoderDecompressStreamDelegate(
            IntPtr state, ref UInt32 availableIn, ref IntPtr nextIn,
            ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        internal delegate void BrotliDecoderDestroyInstanceDelegate(IntPtr state);

        internal delegate UInt32 BrotliDecoderVersionDelegate();

        internal delegate bool BrotliDecoderIsUsedDelegate(IntPtr state);
        internal delegate bool BrotliDecoderIsFinishedDelegate(IntPtr state);
        internal delegate Int32 BrotliDecoderGetErrorCodeDelegate(IntPtr state);
        internal delegate IntPtr BrotliDecoderErrorStringDelegate(Int32 code);
        internal delegate IntPtr BrotliDecoderTakeOutputDelegate(IntPtr state, ref UInt32 size);

        #endregion
    }
}
