# brotli.net
The .net implement for the brotli algorithm.
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
