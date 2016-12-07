using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    public class Brolib
    {
        #region Encoder
        public static IntPtr BrotliEncoderCreateInstance()
        {
            return USE_X86
                ? Brolib32.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                : Brolib64.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool BrotliEncoderSetParameter(
            IntPtr state, BrotliEncoderParameter parameter, UInt32 value)
        {
            return USE_X86
                ? Brolib32.BrotliEncoderSetParameter(state, parameter, value)
                : Brolib64.BrotliEncoderSetParameter(state, parameter, value);
        }

        public static void BrotliEncoderSetCustomDictionary(
            IntPtr state, UInt32 size, IntPtr dict)
        {
            if (USE_X86)
            {
                Brolib32.BrotliEncoderSetCustomDictionary(state, size, dict);
                return;
            }

            Brolib64.BrotliEncoderSetCustomDictionary(state, size, dict);
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation operation, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (USE_X86)
            {
                return Brolib32.BrotliEncoderCompressStream(
                    state,
                    operation,
                    ref availableIn,
                    ref nextIn,
                    ref availableOut,
                    ref nextOut,
                    out totalOut);
            }

            UInt64 availableInL = availableIn;
            UInt64 availableOutL = availableOut;
            UInt64 totalOutL = 0;

            bool result = Brolib64.BrotliEncoderCompressStream(
                state,
                operation,
                ref availableInL,
                ref nextIn,
                ref availableOutL,
                ref nextOut,
                out totalOutL);

            availableIn = (UInt32)availableInL;
            availableOut = (UInt32)availableOutL;
            totalOut = (UInt32)totalOutL;

            return result;
        }

        public static bool BrotliEncoderIsFinished(IntPtr state)
        {
            return USE_X86
                ? Brolib32.BrotliEncoderIsFinished(state)
                : Brolib64.BrotliEncoderIsFinished(state);
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            if (USE_X86)
            {
                Brolib32.BrotliEncoderDestroyInstance(state);
                return;
            }

            Brolib64.BrotliEncoderDestroyInstance(state);
        }

        public static UInt32 BrotliEncoderVersion()
        {
            return USE_X86
                ? Brolib32.BrotliEncoderVersion()
                : Brolib64.BrotliEncoderVersion();
        }
        #endregion

        #region Decoder
        public static IntPtr BrotliDecoderCreateInstance()
        {
            return USE_X86
                ? Brolib32.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                : Brolib64.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public static void BrotliDecoderSetCustomDictionary(
            IntPtr state, UInt32 size, IntPtr dict)
        {
            if (USE_X86)
            {
                Brolib32.BrotliDecoderSetCustomDictionary(state, size, dict);
                return;
            }

            Brolib64.BrotliDecoderSetCustomDictionary(state, size, dict);
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn, ref IntPtr nextIn, ref UInt32 availableOut,
            ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (USE_X86)
            {
                return Brolib32.BrotliDecoderDecompressStream(
                    state,
                    ref availableIn,
                    ref nextIn,
                    ref availableOut,
                    ref nextOut,
                    out totalOut);
            }

            UInt64 availableInL = availableIn;
            UInt64 availableOutL = availableOut;
            UInt64 totalOutL = 0;

            BrotliDecoderResult result = Brolib64.BrotliDecoderDecompressStream(
                state,
                ref availableInL,
                ref nextIn,
                ref availableOutL,
                ref nextOut,
                out totalOutL);

            availableIn = (UInt32)availableInL;
            availableOut = (UInt32)availableOutL;
            totalOut = (UInt32)totalOutL;

            return result;
        }

        public static void BrotliDecoderDestroyInstance(IntPtr state)
        {
            if (USE_X86)
            {
                Brolib32.BrotliDecoderDestroyInstance(state);
                return;
            }

            Brolib64.BrotliDecoderDestroyInstance(state);
        }

        public static UInt32 BrotliDecoderVersion()
        {
            return USE_X86
                ? Brolib32.BrotliDecoderVersion()
                : Brolib64.BrotliDecoderVersion();
        }

        public static bool BrotliDecoderIsUsed(IntPtr state)
        {
            return USE_X86
                ? Brolib32.BrotliDecoderIsUsed(state)
                : Brolib64.BrotliDecoderIsUsed(state);
        }

        public static bool BrotliDecoderIsFinished(IntPtr state)
        {
            return USE_X86
                ? Brolib32.BrotliDecoderIsFinished(state)
                : Brolib64.BrotliDecoderIsFinished(state);
        }

        public static Int32 BrotliDecoderGetErrorCode(IntPtr state)
        {
            return USE_X86
                ? Brolib32.BrotliDecoderGetErrorCode(state)
                : Brolib64.BrotliDecoderGetErrorCode(state);
        }

        public static string  BrotliDecoderErrorString(Int32 code)
        {
            IntPtr result = USE_X86
                ? Brolib32.BrotliDecoderErrorString(code)
                : Brolib64.BrotliDecoderErrorString(code);

            return result != IntPtr.Zero
                ? Marshal.PtrToStringAnsi(result)
                : string.Empty;
        }
        #endregion

        static readonly bool USE_X86 = IntPtr.Size == 4;
    }
}
