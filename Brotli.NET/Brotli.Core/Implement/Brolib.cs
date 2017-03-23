using System;
using System.Runtime.InteropServices;

namespace Brotli
{


    #region Helper Wrapper
    public class Brolib
    {
        static bool UseX86 = IntPtr.Size == 4;
        #region Encoder

        static Brolib()
        {
            InitLibrary();
        }

        public static void InitLibrary()
        {
            if (UseX86)
            {
                BrotliLibWrapper32.InitLibrary();
            }
            else
            {
                BrotliLibWrapper64.InitLibrary();
            }
        }

        public static void ReleaseLibrary()
        {
            if (UseX86)
            {
                BrotliLibWrapper32.ReleaseLibrary();
            }
            else
            {
                BrotliLibWrapper64.ReleaseLibrary();
            }
        }

        public static IntPtr BrotliEncoderCreateInstance()
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.EncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return BrotliLibWrapper64.EncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value)
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.EncoderSetParameter(state, (int)parameter, value);
            }
            else
            {
                return BrotliLibWrapper64.EncoderSetParameter(state, (int)parameter, value);
            }
        }

        public static void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                BrotliLibWrapper32.EncoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                BrotliLibWrapper64.EncoderSetCustomDictionary(state, size, dict);
            }
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.EncoderCompressStream(state, (int)op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = BrotliLibWrapper64.EncoderCompressStream(state, (int)op, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
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
                return BrotliLibWrapper32.EncoderIsFinished(state);
            }
            else
            {
                return BrotliLibWrapper64.EncoderIsFinished(state);
            }
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                BrotliLibWrapper32.EncoderDestroyInstance(state);
            }
            else
            {
                BrotliLibWrapper64.EncoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliEncoderVersion()
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.EncoderVersion();
            }
            else
            {
                return BrotliLibWrapper64.EncoderVersion();
            }
        }


        #endregion
        #region Decoder
        public static IntPtr BrotliDecoderCreateInstance()
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.DecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return BrotliLibWrapper64.DecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                BrotliLibWrapper32.DecoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                BrotliLibWrapper64.DecoderSetCustomDictionary(state, size, dict);
            }
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return (BrotliDecoderResult)BrotliLibWrapper32.DecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = BrotliLibWrapper64.DecoderDecompressStream(state, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
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
                BrotliLibWrapper32.DecoderDestroyInstance(state);
            }
            else
            {
                BrotliLibWrapper64.DecoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliDecoderVersion()
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.DecoderVersion();
            }
            else
            {
                return BrotliLibWrapper64.DecoderVersion();
            }
        }

        public static bool BrotliDecoderIsUsed(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.DecoderIsUsed(state);
            }
            else
            {
                return BrotliLibWrapper64.DecoderIsUsed(state);
            }
        }
        public static bool BrotliDecoderIsFinished(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.DecoderIsFinished(state);
            }
            else
            {
                return BrotliLibWrapper64.DecoderIsFinished(state);
            }

        }
        public static Int32 BrotliDecoderGetErrorCode(IntPtr state)
        {
            if (UseX86)
            {
                return BrotliLibWrapper32.DecoderGetErrorCode(state);
            }
            else
            {
                return BrotliLibWrapper64.DecoderGetErrorCode(state);
            }
        }

        public static String BrotliDecoderErrorString(Int32 code)
        {
            IntPtr r = IntPtr.Zero;
            if (UseX86)
            {
                r = BrotliLibWrapper32.DecoderErrorString(code);
            }
            else
            {
                r = BrotliLibWrapper64.DecoderErrorString(code);
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
