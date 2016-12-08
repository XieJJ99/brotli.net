using System;
using System.Runtime.InteropServices;

using Brotli.Enum;

namespace Brotli
{
    class Brolib64
    {
        #region Encoder
        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliEncoderCreateInstance(
            IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderSetParameter(
            IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderSetCustomDictionary(
            IntPtr state, UInt32 size, IntPtr dict);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt64 availableIn,
            ref IntPtr nextIn, ref UInt64 availableOut, ref IntPtr nextOut, out UInt64 totalOut);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderIsFinished(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderDestroyInstance(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern UInt32 BrotliEncoderVersion();
        #endregion

        #region Decoder
        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliDecoderCreateInstance(
            IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderSetCustomDictionary(
            IntPtr state, UInt64 size, IntPtr dict);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt64 availableIn, ref IntPtr nextIn,
            ref UInt64 availableOut, ref IntPtr nextOut, out UInt64 totalOut);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern UInt32 BrotliDecoderVersion();

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliDecoderIsUsed(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliDecoderIsFinished(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl,CharSet= CharSet.Ansi)]
        internal static extern IntPtr BrotliDecoderErrorString(Int32 code);
        #endregion
    }
}
