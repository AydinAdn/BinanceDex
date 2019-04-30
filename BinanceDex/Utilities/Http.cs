using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinanceDex.Utilities
{
    public interface IHttp
    {
        Task<HttpResponse> GetAsync(string uri);
        Task<HttpResponse> PostAsync(string uri, string data, string contentType);
    }

    public class Http : IHttp
    {
        public Http()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        }

        #region Methods

        public async Task<HttpResponse> GetAsync(string uri)
        {
            Throw.IfNullOrWhiteSpace(uri, nameof(uri));

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                request.Method = "GET";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new HttpResponse
                    {
                        Response = await reader.ReadToEndAsync(),
                        StatusCode = (int) response.StatusCode
                    };
                }
            }
            catch (WebException e)
            {
                using (HttpWebResponse response = (HttpWebResponse) e.Response)
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new HttpResponse
                    {
                        Response = await reader.ReadToEndAsync(),
                        StatusCode = (int) response.StatusCode
                    };
                }
            }
        }

        public async Task<HttpResponse> PostAsync(string uri, string data, string contentType)
        {
            Throw.IfNullOrWhiteSpace(uri, nameof(uri));
            Throw.IfNullOrWhiteSpace(contentType, nameof(contentType));

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                request.Method = "POST";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;

                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new HttpResponse
                    {
                        Response = await reader.ReadToEndAsync(),
                        StatusCode = (int) response.StatusCode
                    };
                }
            }
            catch (WebException e)
            {
                using (HttpWebResponse response = (HttpWebResponse) e.Response)
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new HttpResponse
                    {
                        Response = await reader.ReadToEndAsync(),
                        StatusCode = (int) response.StatusCode
                    };
                }
            }
        }

        #endregion
    }

    public class HttpResponse
    {
        #region Properties

        public string Response { get; set; }
        public int StatusCode { get; set; }

        #endregion
    }
}