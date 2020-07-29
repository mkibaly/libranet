using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Blockcoli.Libra.Net.JsonRPC
{
    public class JsonRpcClient
    {
        Uri uri;
        string jsonrpc = "2.0";

        public JsonRpcClient(string url_)
        {
            uri = new Uri(url_);
        }

        public async Task<string> CallAsync(string method, params object[] _params)
        {
            using (var client = new HttpClient())
            {
                var json = ParseRequest(method, _params);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");//CONTENT-TYPE header

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.SendAsync(request);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public string ParseRequest(string method, object[] parm)
        {
            var obj = new
            {
                jsonrpc = this.jsonrpc,
                method = method,
                @params = parm,
                id = 1
            };

            return JsonConvert.SerializeObject(obj);
        }
    }

}