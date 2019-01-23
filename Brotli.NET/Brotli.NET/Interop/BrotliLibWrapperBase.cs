using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Brotli
{
    internal class BrotliLibbWrapperBase
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr DelegateDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void DelegateDecoderDestroyInstance(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr DelegateDecoderErrorString(int code);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int DelegateDecoderGetErrorCode(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool DelegateDecoderIsFinished(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool DelegateDecoderIsUsed(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void DelegateDecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint DelegateDecoderVersion();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr DelegateEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void DelegateEncoderDestroyInstance(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool DelegateEncoderIsFinished(IntPtr state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void DelegateEncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool DelegateEncoderSetParameter(IntPtr state, int parameter, uint value);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint DelegateEncoderVersion();

        protected static DelegateDecoderCreateInstance _funcDecoderCreateInstance;
        protected static DelegateDecoderDestroyInstance _funcDecoderDestroyInstance;
        protected static DelegateDecoderErrorString _funcDecoderErrorString;
        protected static DelegateDecoderGetErrorCode _funcDecoderGetErrorCode;
        protected static DelegateDecoderIsFinished _funcDecoderIsFinished;
        protected static DelegateDecoderIsUsed _funcDecoderIsUsed;
        protected static DelegateDecoderSetCustomDictionary _funcDecoderSetCustomerDictionary;
        protected static DelegateDecoderVersion _funcDecoderVersion;

        protected static DelegateEncoderCreateInstance _funcEncoderCreateInstance;
        protected static DelegateEncoderDestroyInstance _funcEncoderDestroyInstance;
        protected static DelegateEncoderIsFinished _funcEncoderIsFinished;
        protected static DelegateEncoderSetCustomDictionary _funcEncoderSetCustomerDictionary;
        protected static DelegateEncoderSetParameter _funcEncoderSetParameter;
        protected static DelegateEncoderVersion _funcEncoderVersion;


        internal static T CreateDelegate<T>(IntPtr ptrLibrary, String methodName)
        {
            IntPtr ptrFuncAddress = NativeMethods.GetProcAddress(ptrLibrary, methodName);
            Object funcDelegate = Marshal.GetDelegateForFunctionPointer(ptrFuncAddress, typeof(T));
            return (T)funcDelegate;
        }


        public static IntPtr GetModuleHandle(String moduleName)
        {
            IntPtr r = IntPtr.Zero;
            foreach (ProcessModule mod in Process.GetCurrentProcess().Modules)
            {
                if (mod.ModuleName == moduleName)
                {
                    r = mod.BaseAddress;
                    break;
                }
            }
            return r;
        }

        public static IntPtr DecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return _funcDecoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static void DecoderDestroyInstance(IntPtr state)
        {
            _funcDecoderDestroyInstance(state);
        }
        public static IntPtr DecoderErrorString(int code)
        {
            return _funcDecoderErrorString(code);
        }

        public static int DecoderGetErrorCode(IntPtr state)
        {
            return _funcDecoderGetErrorCode(state);
        }

        public static bool DecoderIsFinished(IntPtr state)
        {
            return _funcDecoderIsFinished(state);
        }
        public static bool DecoderIsUsed(IntPtr state)
        {
            return _funcDecoderIsUsed(state);
        }
        public static void DecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            _funcDecoderSetCustomerDictionary(state, size, dict);
        }
        public static uint DecoderVersion()
        {
            return _funcDecoderVersion();
        }

        public static IntPtr EncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return _funcEncoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static void EncoderDestroyInstance(IntPtr state)
        {
            _funcEncoderDestroyInstance(state);
        }
        public static bool EncoderIsFinished(IntPtr state)
        {
            return _funcEncoderIsFinished(state);
        }
        public static void EncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict)
        {
            _funcEncoderSetCustomerDictionary(state, size, dict);
        }
        public static bool EncoderSetParameter(IntPtr state, int parameter, uint value)
        {
            return _funcEncoderSetParameter(state, parameter, value);
        }
        public static uint EncoderVersion()
        {
            return _funcEncoderVersion();
        }
    }
}
