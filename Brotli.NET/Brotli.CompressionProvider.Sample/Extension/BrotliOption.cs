using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brotli.CompressionProvider.Sample
{
    public class BrotliOption : IOptions<BrotliOption>
    {
        public BrotliOption()
        {

        }
        public BrotliOption(uint quality,uint window)
        {
            Quality = quality;
            Window = window;
        }
        /// <summary>
        /// Set the compress quality(0~11)
        /// </summary>
        public uint Quality { get;  set; } = 5;
        /// <summary>
        /// Set the compress LGWin(10~24)
        /// </summary>
        public uint Window { get;  set; } = 22;
        BrotliOption IOptions<BrotliOption>.Value => this;
    }
}
