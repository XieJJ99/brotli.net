using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Brotli
{
    #region Helper reference class
    internal class BrotliHelperRef32
    {
        #region delegates
        delegate IntPtr DelegateDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        delegate int DelegateDecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);
        delegate void DelegateDecoderDestroyInstance(IntPtr state);
        delegate IntPtr DelegateDecoderErrorString(int code);
        delegate int DelegateDecoderGetErrorCode(IntPtr state);
        delegate bool DelegateDecoderIsFinished(IntPtr state);
        delegate bool DelegateDecoderIsUsed(IntPtr state);
        delegate void DelegateDecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        delegate uint DelegateDecoderVersion();

        delegate bool DelegateEncoderCompressStream(IntPtr state, int op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut);
        delegate IntPtr DelegateEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        delegate void DelegateEncoderDestroyInstance(IntPtr state);
        delegate bool DelegateEncoderIsFinished(IntPtr state);
        delegate void DelegateEncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        delegate bool DelegateEncoderSetParameter(IntPtr state, int parameter, uint value);
        delegate uint DelegateEncoderVersion();

        static DelegateDecoderCreateInstance _funcDecoderCreateInstance;
        static DelegateDecoderDecompressStream _funcDecoderDecompressStream;
        static DelegateDecoderDestroyInstance _funcDecoderDestroyInstance;
        static DelegateDecoderErrorString _funcDecoderErrorString;
        static DelegateDecoderGetErrorCode _funcDecoderGetErrorCode;
        static DelegateDecoderIsFinished _funcDecoderIsFinished;
        static DelegateDecoderIsUsed _funcDecoderIsUsed;
        static DelegateDecoderSetCustomDictionary _funcDecoderSetCustomerDictionary;
        static DelegateDecoderVersion _funcDecoderVersion;

        static DelegateEncoderCompressStream _funcEncoderCompressStream;
        static DelegateEncoderCreateInstance _funcEncoderCreateInstance;
        static DelegateEncoderDestroyInstance _funcEncoderDestroyInstance;
        static DelegateEncoderIsFinished _funcEncoderIsFinished;
        static DelegateEncoderSetCustomDictionary _funcEncoderSetCustomerDictionary;
        static DelegateEncoderSetParameter _funcEncoderSetParameter;
        static DelegateEncoderVersion _funcEncoderVersion;

        static T CreateDelegate<T>(Type target, String methodName)
        {
            Type t = typeof(T);
            Object od = Delegate.CreateDelegate(t, target, methodName);
            return (T)od;
        }
        static BrotliHelperRef32()
        {
            
            var useNetFx2 =Environment.Version.Major < 4;
            var dirPath = typeof(BrotliStream).Assembly.Location;
            dirPath = dirPath.Substring(0,dirPath.LastIndexOfAny(new Char[] { '\\', '/' }));
            var destAssemblyName = String.Format("{0}\\{1}", dirPath, useNetFx2 ? "broclr2_32.dll" : "broclr4_32.dll");
            var destAssembly = Assembly.LoadFrom(destAssemblyName);
            var destType = destAssembly.GetType("Brotli.BrotliHelper32");
            _funcDecoderCreateInstance = CreateDelegate<DelegateDecoderCreateInstance>(destType, "DecoderCreateInstance");
            _funcDecoderDecompressStream = CreateDelegate<DelegateDecoderDecompressStream>(destType, "DecoderDecompressStream");
            _funcDecoderDestroyInstance = CreateDelegate<DelegateDecoderDestroyInstance>(destType,"DecoderDestroyInstance");
            _funcDecoderErrorString = CreateDelegate<DelegateDecoderErrorString>(destType, "DecoderErrorString");
            _funcDecoderGetErrorCode = CreateDelegate<DelegateDecoderGetErrorCode>(destType, "DecoderGetErrorCode");
            _funcDecoderIsFinished = CreateDelegate<DelegateDecoderIsFinished>(destType, "DecoderIsFinished");
            _funcDecoderIsUsed = CreateDelegate<DelegateDecoderIsUsed>(destType, "DecoderIsUsed");
            _funcDecoderSetCustomerDictionary = CreateDelegate<DelegateDecoderSetCustomDictionary>(destType, "DecoderSetCustomDictionary");
            _funcDecoderVersion = CreateDelegate<DelegateDecoderVersion>(destType, "DecoderVersion");

            _funcEncoderCompressStream= CreateDelegate<DelegateEncoderCompressStream>(destType, "EncoderCompressStream");
            _funcEncoderCreateInstance= CreateDelegate<DelegateEncoderCreateInstance>(destType,  "EncoderCreateInstance");
            _funcEncoderDestroyInstance= CreateDelegate<DelegateEncoderDestroyInstance>(destType, "EncoderDestroyInstance");
            _funcEncoderIsFinished= CreateDelegate<DelegateEncoderIsFinished>(destType, "EncoderIsFinished");
            _funcEncoderSetCustomerDictionary= CreateDelegate<DelegateEncoderSetCustomDictionary>(destType, "EncoderSetCustomDictionary");
            _funcEncoderSetParameter= CreateDelegate<DelegateEncoderSetParameter>(destType, "EncoderSetParameter");
            _funcEncoderVersion= CreateDelegate<DelegateEncoderVersion>(destType, "EncoderVersion");
        }
        #endregion

        public static IntPtr DecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return _funcDecoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static int DecoderDecompressStream(IntPtr state, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return _funcDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
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
        public static bool EncoderCompressStream(IntPtr state, int op, ref uint availableIn, ref IntPtr nextIn, ref uint availableOut, ref IntPtr nextOut, out uint totalOut)
        {
            return _funcEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
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

    internal class BrotliHelperRef64
    {
        #region delegates
        delegate IntPtr DelegateDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        delegate int DelegateDecoderDecompressStream(IntPtr state, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut);
        delegate void DelegateDecoderDestroyInstance(IntPtr state);
        delegate IntPtr DelegateDecoderErrorString(int code);
        delegate int DelegateDecoderGetErrorCode(IntPtr state);
        delegate bool DelegateDecoderIsFinished(IntPtr state);
        delegate bool DelegateDecoderIsUsed(IntPtr state);
        delegate void DelegateDecoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        delegate uint DelegateDecoderVersion();

        delegate bool DelegateEncoderCompressStream(IntPtr state, int op, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut);
        delegate IntPtr DelegateEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);
        delegate void DelegateEncoderDestroyInstance(IntPtr state);
        delegate bool DelegateEncoderIsFinished(IntPtr state);
        delegate void DelegateEncoderSetCustomDictionary(IntPtr state, uint size, IntPtr dict);
        delegate bool DelegateEncoderSetParameter(IntPtr state, int parameter, uint value);
        delegate uint DelegateEncoderVersion();

        static DelegateDecoderCreateInstance _funcDecoderCreateInstance;
        static DelegateDecoderDecompressStream _funcDecoderDecompressStream;
        static DelegateDecoderDestroyInstance _funcDecoderDestroyInstance;
        static DelegateDecoderErrorString _funcDecoderErrorString;
        static DelegateDecoderGetErrorCode _funcDecoderGetErrorCode;
        static DelegateDecoderIsFinished _funcDecoderIsFinished;
        static DelegateDecoderIsUsed _funcDecoderIsUsed;
        static DelegateDecoderSetCustomDictionary _funcDecoderSetCustomerDictionary;
        static DelegateDecoderVersion _funcDecoderVersion;

        static DelegateEncoderCompressStream _funcEncoderCompressStream;
        static DelegateEncoderCreateInstance _funcEncoderCreateInstance;
        static DelegateEncoderDestroyInstance _funcEncoderDestroyInstance;
        static DelegateEncoderIsFinished _funcEncoderIsFinished;
        static DelegateEncoderSetCustomDictionary _funcEncoderSetCustomerDictionary;
        static DelegateEncoderSetParameter _funcEncoderSetParameter;
        static DelegateEncoderVersion _funcEncoderVersion;

        static T CreateDelegate<T>(Type target, String methodName)
        {
            Type t = typeof(T);
            Object od = Delegate.CreateDelegate(t, target, methodName);
            return (T)od;
        }
        static BrotliHelperRef64()
        {

            var useNetFx2 = Environment.Version.Major < 4;
            var dirPath = typeof(BrotliStream).Assembly.Location;
            dirPath = dirPath.Substring(0, dirPath.LastIndexOfAny(new Char[] { '\\', '/' }));
            var destAssemblyName = String.Format("{0}\\{1}", dirPath, useNetFx2 ? "broclr2_64.dll" : "broclr4_64.dll");
            var destAssembly = Assembly.LoadFrom(destAssemblyName);
            var destType = destAssembly.GetType("Brotli.BrotliHelper64");
            _funcDecoderCreateInstance = CreateDelegate<DelegateDecoderCreateInstance>(destType, "DecoderCreateInstance");
            _funcDecoderDecompressStream = CreateDelegate<DelegateDecoderDecompressStream>(destType, "DecoderDecompressStream");
            _funcDecoderDestroyInstance = CreateDelegate<DelegateDecoderDestroyInstance>(destType, "DecoderDestroyInstance");
            _funcDecoderErrorString = CreateDelegate<DelegateDecoderErrorString>(destType, "DecoderErrorString");
            _funcDecoderGetErrorCode = CreateDelegate<DelegateDecoderGetErrorCode>(destType, "DecoderGetErrorCode");
            _funcDecoderIsFinished = CreateDelegate<DelegateDecoderIsFinished>(destType, "DecoderIsFinished");
            _funcDecoderIsUsed = CreateDelegate<DelegateDecoderIsUsed>(destType, "DecoderIsUsed");
            _funcDecoderSetCustomerDictionary = CreateDelegate<DelegateDecoderSetCustomDictionary>(destType, "DecoderSetCustomerDictionary");
            _funcDecoderVersion = CreateDelegate<DelegateDecoderVersion>(destType, "DecoderVersion");

            _funcEncoderCompressStream = CreateDelegate<DelegateEncoderCompressStream>(destType, "EncoderCompressStream");
            _funcEncoderCreateInstance = CreateDelegate<DelegateEncoderCreateInstance>(destType, "EncoderCreateInstance");
            _funcEncoderDestroyInstance = CreateDelegate<DelegateEncoderDestroyInstance>(destType, "EncoderDestroyInstance");
            _funcEncoderIsFinished = CreateDelegate<DelegateEncoderIsFinished>(destType, "EncoderIsFinished");
            _funcEncoderSetCustomerDictionary = CreateDelegate<DelegateEncoderSetCustomDictionary>(destType, "EncoderSetCustomDictionary");
            _funcEncoderSetParameter = CreateDelegate<DelegateEncoderSetParameter>(destType, "EncoderSetParameter");
            _funcEncoderVersion = CreateDelegate<DelegateEncoderVersion>(destType, "EncoderVersion");
        }
        #endregion

        public static IntPtr DecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque)
        {
            return _funcDecoderCreateInstance(allocFunc, freeFunc, opaque);
        }
        public static int DecoderDecompressStream(IntPtr state, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return _funcDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
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
        public static bool EncoderCompressStream(IntPtr state, int op, ref ulong availableIn, ref IntPtr nextIn, ref ulong availableOut, ref IntPtr nextOut, out ulong totalOut)
        {
            return _funcEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
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
