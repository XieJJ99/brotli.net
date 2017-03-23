using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    internal class BrotliLibWrapper32: BrotliLibbWrapperBase
    {
        #region delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int DelegateDecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool DelegateEncoderCompressStream(IntPtr state, int op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);

        static DelegateDecoderDecompressStream _funcDecoderDecompressStream;

        static DelegateEncoderCompressStream _funcEncoderCompressStream;

        const string LibraryName = "brolib32.dll";
        static IntPtr NativeLibraryPtr = IntPtr.Zero;
        internal static void InitLibrary()
        {
            if (NativeLibraryPtr != IntPtr.Zero) return;
            var ptrLibrary = GetModuleHandle(LibraryName);
            if (ptrLibrary == IntPtr.Zero)
            {
                ptrLibrary = NativeMethods.LoadLibrary(LibraryName);
                if (ptrLibrary == IntPtr.Zero)
                {
                    throw new System.ComponentModel.Win32Exception();
                }
            }

            NativeLibraryPtr = ptrLibrary;
            _funcDecoderCreateInstance = CreateDelegate<DelegateDecoderCreateInstance>(ptrLibrary, "BrotliDecoderCreateInstance");
            _funcDecoderDecompressStream = CreateDelegate<DelegateDecoderDecompressStream>(ptrLibrary, "BrotliDecoderDecompressStream");
            _funcDecoderDestroyInstance = CreateDelegate<DelegateDecoderDestroyInstance>(ptrLibrary, "BrotliDecoderDestroyInstance");
            _funcDecoderErrorString = CreateDelegate<DelegateDecoderErrorString>(ptrLibrary, "BrotliDecoderErrorString");
            _funcDecoderGetErrorCode = CreateDelegate<DelegateDecoderGetErrorCode>(ptrLibrary, "BrotliDecoderGetErrorCode");
            _funcDecoderIsFinished = CreateDelegate<DelegateDecoderIsFinished>(ptrLibrary, "BrotliDecoderIsFinished");
            _funcDecoderIsUsed = CreateDelegate<DelegateDecoderIsUsed>(ptrLibrary, "BrotliDecoderIsUsed");
            _funcDecoderSetCustomerDictionary = CreateDelegate<DelegateDecoderSetCustomDictionary>(ptrLibrary, "BrotliDecoderSetCustomDictionary");
            _funcDecoderVersion = CreateDelegate<DelegateDecoderVersion>(ptrLibrary, "BrotliDecoderVersion");

            _funcEncoderCompressStream = CreateDelegate<DelegateEncoderCompressStream>(ptrLibrary, "BrotliEncoderCompressStream");
            _funcEncoderCreateInstance = CreateDelegate<DelegateEncoderCreateInstance>(ptrLibrary, "BrotliEncoderCreateInstance");
            _funcEncoderDestroyInstance = CreateDelegate<DelegateEncoderDestroyInstance>(ptrLibrary, "BrotliEncoderDestroyInstance");
            _funcEncoderIsFinished = CreateDelegate<DelegateEncoderIsFinished>(ptrLibrary, "BrotliEncoderIsFinished");
            _funcEncoderSetCustomerDictionary = CreateDelegate<DelegateEncoderSetCustomDictionary>(ptrLibrary, "BrotliEncoderSetCustomDictionary");
            _funcEncoderSetParameter = CreateDelegate<DelegateEncoderSetParameter>(ptrLibrary, "BrotliEncoderSetParameter");
            _funcEncoderVersion = CreateDelegate<DelegateEncoderVersion>(ptrLibrary, "BrotliEncoderVersion");

        }

        internal static void ReleaseLibrary()
        {
            if (NativeLibraryPtr != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(NativeLibraryPtr);

            }
            NativeLibraryPtr = IntPtr.Zero;
            _funcDecoderCreateInstance = null;
            _funcDecoderDecompressStream = null;
            _funcDecoderDestroyInstance = null;
            _funcDecoderErrorString = null;
            _funcDecoderGetErrorCode = null;
            _funcDecoderIsFinished = null;
            _funcDecoderIsUsed = null;
            _funcDecoderSetCustomerDictionary = null;
            _funcDecoderVersion = null;

            _funcEncoderCompressStream = null;
            _funcEncoderCreateInstance = null;
            _funcEncoderDestroyInstance = null;
            _funcEncoderIsFinished = null;
            _funcEncoderSetCustomerDictionary = null;
            _funcEncoderSetParameter = null;
            _funcEncoderVersion = null;
        }

        static BrotliLibWrapper32()
        {
            InitLibrary();
        }
        #endregion


        public static int DecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return _funcDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }
        public static bool EncoderCompressStream(IntPtr state, int op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return _funcEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

    }
}
