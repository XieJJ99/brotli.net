# Brotli-dot-NET

C# bindings for the Windows native libraries of the Brotli compression algorithm.  
Re-wrote from [XieJJ99/brotli.net](https://github.com/XieJJ99/brotli.net) with buffers in mind (instead of streams, but will likely support both in the future).

------

To compress a buffer:

```C#
byte[] flatBytes = GetMyAwesomeDataToCompress();
byte[] output = new byte[flatBytes.Length];

int outputLength = 0;
using (BrotliNET brotli = new BrotliNET(CompressionMode.Compress))
{
    brotli.Quality = 11;
    brotli.Window = 22;

    outputLength = brotli.Compress(flatBytes, 0, flatBytes.Length, output);
}

// The first 'ouputLength' bytes of the output buffer are the valid ones
WriteMyAwesomeCompressedData(output, output.Length);
```

To decompress a buffer:

```C#
byte[] compressedBytes = GetMyAwesomeDataToDecompress();
byte[] output = new byte[compressedBytes.Length * 4];

int outputLength = 0;
using (BrotliNET brotli = new BrotliNET(CompressionMode.Decompress))
{
    outputLength = brotli.Decompress(
          compressedBytes, 0, compressedBytes.Length, output);
}

// The first 'ouputLength' bytes of the output buffer are the valid ones
WriteMyAwesomeDecompressedData(output, output.Length);
```