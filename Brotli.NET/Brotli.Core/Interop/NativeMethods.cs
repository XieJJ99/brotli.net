using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Brotli
{
    static class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllFilePath);

        [DllImport("kernel32.dll",CharSet = CharSet.Ansi,SetLastError =true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
