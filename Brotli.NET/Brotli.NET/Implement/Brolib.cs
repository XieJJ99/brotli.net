using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brotli
{
    public class Brolib
    {
        static bool UseX86 = IntPtr.Size == 4;
        #region Encoder
        public static IntPtr BrotliEncoderCreateInstance()
        {
            if (UseX86)
            {
                return Brolib32.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return Brolib64.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value)
        {
            if (UseX86)
            {
                return Brolib32.BrotliEncoderSetParameter(state, parameter, value);
            }
            else
            {
                return Brolib64.BrotliEncoderSetParameter(state, parameter, value);
            }
        }

        public static void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                Brolib32.BrotliEncoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                Brolib64.BrotliEncoderSetCustomDictionary(state, size, dict);
            }
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return Brolib32.BrotliEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = Brolib64.BrotliEncoderCompressStream(state, op, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
                availableIn = (UInt32)availableInL;
                availableOut = (UInt32)availableOutL;
                totalOut = (UInt32)totalOutL;
                return r;
            }
        }

        public static bool BrotliEncoderIsFinished(IntPtr state)
        {
            if (UseX86)
            {
                return Brolib32.BrotliEncoderIsFinished(state);
            }
            else
            {
                return Brolib64.BrotliEncoderIsFinished(state);
            }
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                Brolib32.BrotliEncoderDestroyInstance(state);
            }
            else
            {
                Brolib64.BrotliEncoderDestroyInstance(state);
            }
        }
        #endregion
        #region Decoder
        public static IntPtr BrotliDecoderCreateInstance()
        {
            if (UseX86)
            {
                return Brolib32.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return Brolib64.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                Brolib32.BrotliDecoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                Brolib64.BrotliDecoderSetCustomDictionary(state, size, dict);
            }
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return Brolib32.BrotliDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = Brolib64.BrotliDecoderDecompressStream(state, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
                availableIn = (UInt32)availableInL;
                availableOut = (UInt32)availableOutL;
                totalOut = (UInt32)totalOutL;
                return r;
            }
        }

        public static void BrotliDecoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                Brolib32.BrotliDecoderDestroyInstance(state);
            }
            else
            {
                Brolib64.BrotliDecoderDestroyInstance(state);
            }
        }

        #endregion
    }
}
