using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    class Brolib32
    {
        #region Encoder
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderIsFinished(IntPtr state);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderDestroyInstance(IntPtr state);
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern UInt32 BrotliEncoderVersion();
        #endregion
        #region Decoder
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn, ref IntPtr nextIn,
            ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern UInt32 BrotliDecoderVersion();

        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliDecoderIsUsed(IntPtr state);
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliDecoderIsFinished(IntPtr state);
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);
        [DllImport("brolib32.dll", CallingConvention = CallingConvention.Cdecl,CharSet= CharSet.Ansi)]
        internal static extern IntPtr BrotliDecoderErrorString(Int32 code);
        #endregion
    }
}
