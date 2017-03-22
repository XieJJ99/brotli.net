# brotli.net
The .net implement of the brotli algorithm.

To compress a stream to brotli data:

       public Byte[] Encode(Byte[] input)
       {
           Byte[] output = null;
           using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
           using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
           using (BrotliStream bs = new BrotliStream(msOutput, System.IO.Compression.CompressionMode.Compress))
           {
               bs.SetQuality(11);
               bs.SetWindow(22);
               msInput.CopyTo(bs);
               bs.Close();
               output = msOutput.ToArray();
               return output;
           }
       }

To decompress a brotli stream:

       public Byte[] Decode(Byte[] input)
       {
           using (System.IO.MemoryStream msInput = new System.IO.MemoryStream(input))
           using (BrotliStream bs = new BrotliStream(msInput, System.IO.Compression.CompressionMode.Decompress))
           using (System.IO.MemoryStream msOutput = new System.IO.MemoryStream())
           {
               bs.CopyTo(msOutput);
               msOutput.Seek(0, System.IO.SeekOrigin.Begin);
               output = msOutput.ToArray();
               return output;
           }

       }

To support dynamic compress in web applications,add the code like this in the Global.asax.cs:

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
                    else
                    if (acceptEncodings.Contains("deflate"))
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


From version v1.0.15, Brotli.NET migrated to C++/CLR, all related assemblies can be updated/removed when inside a web application. 
