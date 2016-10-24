using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Brotli
{
    class Brolib64
    {
        #region Encoder
        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt64 availableIn,
            ref IntPtr nextIn, ref UInt64 availableOut, ref IntPtr nextOut, out UInt64 totalOut);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool BrotliEncoderIsFinished(IntPtr state);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliEncoderDestroyInstance(IntPtr state);
        #endregion
        #region Decoder
        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderSetCustomDictionary(IntPtr state, UInt64 size, IntPtr dict);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt64 availableIn, ref IntPtr nextIn,
            ref UInt64 availableOut, ref IntPtr nextOut, out UInt64 totalOut);

        [DllImport("brolib64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

        #endregion
    }
}
