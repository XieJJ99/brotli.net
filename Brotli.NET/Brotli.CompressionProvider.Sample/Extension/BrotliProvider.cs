using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Brotli.CompressionProvider.Sample
{
    public class BrotliProvider : ICompressionProvider
    {

        /// <summary>
        /// Creates a new instance of <see cref="BrotliCompressionProvider"/> with options.
        /// </summary>
        /// <param name="options"></param>
        public BrotliProvider(IOptions<BrotliOption> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
        }

        private BrotliOption Options { get; }

        /// <inheritdoc />
        public string EncodingName { get; } = "br";

        /// <inheritdoc />
        public bool SupportsFlush { get; } = true;

        /// <inheritdoc />
        public Stream CreateStream(Stream outputStream)
        {
            var bs = new BrotliStream(outputStream, System.IO.Compression.CompressionMode.Compress , leaveOpen: true);
            bs.SetQuality(Options.Quality);
            bs.SetWindow(Options.Window);
            return bs;
        }
    }
}
