﻿using System;

namespace Brotli.Exceptions
{
    public class BrotliException : Exception
    {
        public BrotliException()
            : base() { }

        public BrotliException(String message)
            : base(message) { }

        public BrotliException(String message, Exception innerException)
            : base(message, innerException) { }
    }
}
