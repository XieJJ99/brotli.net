using System;
using System.Runtime.InteropServices;

namespace Brotli
{
    internal class BrotliLibWrapper64: BrotliLibbWrapperBase
    {
        #region delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int DelegateDecoderDecompressStream(IntPtr state, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool DelegateEncoderCompressStream(IntPtr state, int op, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut);

        protected static DelegateDecoderDecompressStream _funcDecoderDecompressStream;
        protected static DelegateEncoderCompressStream _funcEncoderCompressStream;


        const string LibraryName = "brolib64.dll";
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



        static BrotliLibWrapper64()
        {

            InitLibrary();
        }
        #endregion


        public static int DecoderDecompressStream(IntPtr state, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return _funcDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

        public static bool EncoderCompressStream(IntPtr state, int op, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return _funcEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }


    }
    
}
