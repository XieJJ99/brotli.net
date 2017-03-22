using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    #region Helper reference class
    internal class BrotliHelperRef32
    {
        public static IntPtr DecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return BrotliHelper32.DecoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static int DecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return BrotliHelper32.DecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }
        public static void DecoderDestroyInstance(IntPtr state)
        {
            BrotliHelper32.DecoderDestroyInstance(state);
        }
        public static IntPtr DecoderErrorString(int code)
        {
            return BrotliHelper32.DecoderErrorString(code);
        }

        public static int DecoderGetErrorCode(IntPtr state)
        {
            return BrotliHelper32.DecoderGetErrorCode(state);
        }

        public static bool DecoderIsFinished(IntPtr state)
        {
            return BrotliHelper32.DecoderIsFinished(state);
        }
        public static bool DecoderIsUsed(IntPtr state)
        {
            return BrotliHelper32.DecoderIsUsed(state);
        }
        public static void DecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            BrotliHelper32.DecoderSetCustomDictionary(state, size, dict);
        }
        public static uint DecoderVersion()
        {
            return BrotliHelper32.DecoderVersion();
        }
        public static bool EncoderCompressStream(IntPtr state, int op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return BrotliHelper32.EncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

        public static IntPtr EncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return BrotliHelper32.EncoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static void EncoderDestroyInstance(IntPtr state)
        {
            BrotliHelper32.EncoderDestroyInstance(state);
        }
        public static bool EncoderIsFinished(IntPtr state)
        {
            return BrotliHelper32.EncoderIsFinished(state);
        }
        public static void EncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            BrotliHelper32.EncoderSetCustomDictionary(state, size, dict);
        }
        public static bool EncoderSetParameter(IntPtr state, int parameter, uint value)
        {
            return BrotliHelper32.EncoderSetParameter(state, parameter, value);
        }
        public static uint EncoderVersion()
        {
            return BrotliHelper32.EncoderVersion();
        }
    }

    internal class BrotliHelperRef64
    {
        public static IntPtr DecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return BrotliHelper64.DecoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static int DecoderDecompressStream(IntPtr state, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return BrotliHelper64.DecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }
        public static void DecoderDestroyInstance(IntPtr state)
        {
            BrotliHelper64.DecoderDestroyInstance(state);
        }
        public static IntPtr DecoderErrorString(int code)
        {
            return BrotliHelper64.DecoderErrorString(code);
        }

        public static int DecoderGetErrorCode(IntPtr state)
        {
            return BrotliHelper64.DecoderGetErrorCode(state);
        }

        public static bool DecoderIsFinished(IntPtr state)
        {
            return BrotliHelper64.DecoderIsFinished(state);
        }
        public static bool DecoderIsUsed(IntPtr state)
        {
            return BrotliHelper64.DecoderIsUsed(state);
        }
        public static void DecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            BrotliHelper64.DecoderSetCustomDictionary(state, size, dict);
        }
        public static uint DecoderVersion()
        {
            return BrotliHelper64.DecoderVersion();
        }
        public static bool EncoderCompressStream(IntPtr state, int op, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return BrotliHelper64.EncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

        public static IntPtr EncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return BrotliHelper64.EncoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static void EncoderDestroyInstance(IntPtr state)
        {
            BrotliHelper64.EncoderDestroyInstance(state);
        }
        public static bool EncoderIsFinished(IntPtr state)
        {
            return BrotliHelper64.EncoderIsFinished(state);
        }
        public static void EncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            BrotliHelper64.EncoderSetCustomDictionary(state, size, dict);
        }
        public static bool EncoderSetParameter(IntPtr state, int parameter, uint value)
        {
            return BrotliHelper64.EncoderSetParameter(state, parameter, value);
        }
        public static uint EncoderVersion()
        {
            return BrotliHelper64.EncoderVersion();
        }
    }
    #endregion

    #region Helper Wrapper
    public class Brolib
    {
        static bool UseX86 = IntPtr.Size == 4;
        #region Encoder
        public static IntPtr BrotliEncoderCreateInstance()
        {
            if (UseX86)
            {
                return BrotliHelperRef32.EncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return BrotliHelperRef64.EncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value)
        {
            if (UseX86)
            {
                return BrotliHelperRef32.EncoderSetParameter(state, (int)parameter, value);
            }
            else
            {
                return BrotliHelperRef64.EncoderSetParameter(state, (int)parameter, value);
            }
        }

        public static void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                BrotliHelperRef32.EncoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                BrotliHelperRef64.EncoderSetCustomDictionary(state, size, dict);
            }
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return BrotliHelperRef32.EncoderCompressStream(state, (int)op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = BrotliHelperRef64.EncoderCompressStream(state, (int)op, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
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
                return BrotliHelperRef32.EncoderIsFinished(state);
            }
            else
            {
                return BrotliHelperRef64.EncoderIsFinished(state);
            }
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                BrotliHelperRef32.EncoderDestroyInstance(state);
            }
            else
            {
                BrotliHelperRef64.EncoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliEncoderVersion()
        {
            if (UseX86)
            {
                return BrotliHelperRef32.EncoderVersion();
            }
            else
            {
                return BrotliHelperRef64.EncoderVersion();
            }
        }


        #endregion
        #region Decoder
        public static IntPtr BrotliDecoderCreateInstance()
        {
            if (UseX86)
            {
                return BrotliHelperRef32.DecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return BrotliHelperRef64.DecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                BrotliHelperRef32.DecoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                BrotliHelperRef64.DecoderSetCustomDictionary(state, size, dict);
            }
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return (BrotliDecoderResult)BrotliHelperRef32.DecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = BrotliHelperRef64.DecoderDecompressStream(state, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
                availableIn = (UInt32)availableInL;
                availableOut = (UInt32)availableOutL;
                totalOut = (UInt32)totalOutL;
                return (BrotliDecoderResult)r;
            }
        }

        public static void BrotliDecoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                BrotliHelperRef32.DecoderDestroyInstance(state);
            }
            else
            {
                BrotliHelperRef64.DecoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliDecoderVersion()
        {
            if (UseX86)
            {
                return BrotliHelperRef32.DecoderVersion();
            }
            else
            {
                return BrotliHelperRef64.DecoderVersion();
            }
        }

        public static bool BrotliDecoderIsUsed(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliHelperRef32.DecoderIsUsed(state);
            }
            else
            {
                return BrotliHelperRef64.DecoderIsUsed(state);
            }
        }
        public static bool BrotliDecoderIsFinished(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliHelperRef32.DecoderIsFinished(state);
            }
            else
            {
                return BrotliHelperRef64.DecoderIsFinished(state);
            }

        }
        public static Int32 BrotliDecoderGetErrorCode(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliHelperRef32.DecoderGetErrorCode(state);
            }
            else
            {
                return BrotliHelperRef64.DecoderGetErrorCode(state);
            }
        }

        public static String BrotliDecoderErrorString(Int32 code)
        {
            IntPtr r = IntPtr.Zero;
            if (UseX86)
            {
                r = BrotliHelperRef32.DecoderErrorString(code);
            }
            else
            {
                r = BrotliHelperRef64.DecoderErrorString(code);
            }

            if (r != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(r);
            }
            return String.Empty;


        }


        #endregion
    }
    #endregion
}
