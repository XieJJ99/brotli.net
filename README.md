# brotli.net
The .net implement of the brotli algorithm,provide similar interface to Google offical API.

Quality and window control is supported.

Supported and tested on:
- Dotnet standard 2(.NET Framework [v4.6.1] and .net core [2] above)
- Windows/Linux/MacOSX
- .NET Framework v4.5

Besides quality controll,the library use the native runtime and its performance should be better than System.IO.Compress.BrotliStream.
## Supporting platform: .NET Standard 2(Windows/Linux/Mac OSX)

## Usage
### .net core compression
For compression provider sample, please check the Brotli.CompressionProvider.Sample folder.
### Compress

```C#
using Brotli;

//the first parameter controls the quality(0-11)
var compressedData=inputBytes.CompressToBrotli(5,22);
//or
var compressedData=inputSteam.CompressToBrotli();
//or
inputStream.CompressToBrotli(destStream)
```       

### Decompress

```C#
var decompressedData=inputBytes.DecompressFromBrotli();
//or
var decompressedData=inputSteam.DecompressFromBrotli();
//or
inputSteam.DecompressFromBrotli(destStream);
```

### Dynamic compress support(Legacy .NET Mvc)

```C#
protected void Application_PostAcquireRequestState(object sender, EventArgs e)
{
    var app = Context.ApplicationInstance;
    String acceptEncodings = app.Request.Headers.Get("Accept-Encoding");
    if (!String.IsNullOrEmpty(acceptEncodings))
    {
        System.IO.Stream baseStream = app.Response.Filter;
        acceptEncodings = acceptEncodings.ToLower();
        if (acceptEncodings.Contains("br") || acceptEncodings.Contains("brotli"))
        {
            app.Response.Filter = new Brotli.BrotliStream(baseStream, System.IO.Compression.CompressionMode.Compress);
            app.Response.AppendHeader("Content-Encoding", "br");
        }
        else if (acceptEncodings.Contains("deflate"))
        {
            app.Response.Filter = new System.IO.Compression.DeflateStream(baseStream, System.IO.Compression.CompressionMode.Compress);
            app.Response.AppendHeader("Content-Encoding", "deflate");
        }
        else if (acceptEncodings.Contains("gzip"))
        {
            app.Response.Filter = new System.IO.Compression.GZipStream(baseStream, System.IO.Compression.CompressionMode.Compress);
            app.Response.AppendHeader("Content-Encoding", "gzip");
        }
    }
}      	
```

## Legacy version
For .net framework below v4.5, you can use version v2.0.2.4.
For .net framework below v3.5, you can use version v1.0.19.

## License
MIT