using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public interface IHttpWebRequestMethod
    {
        Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, string key, int timeout=60);
        Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, string key, int timeout = 60);
        //Delete();
        //Put();
        //Patch();
    }
    public class HttpWebRequestMethod : IHttpWebRequestMethod
    {
        private readonly ITokenService _tokenService;
        public HttpWebRequestMethod(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        private ConcurrentDictionary<string, string> tokenDic = new ConcurrentDictionary<string, string>();

        public async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, string key, int timeout = 60)
        {
            HttpResponseMessage httpResponseMessage = null;
            request.Method = HttpMethod.Get;
                       
            httpResponseMessage = await SendAsync(request, key, timeout);
            
            return httpResponseMessage;
        }
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, string key, int timeout = 60)
        {
            HttpResponseMessage httpResponseMessage = null;
            request.Method = HttpMethod.Post;
            
            httpResponseMessage = await SendAsync(request, key, timeout);
           
            return httpResponseMessage;
        }



        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, string key, int timeout = 60)
        {
            HttpResponseMessage httpResponseMessage = null;

            string accessToken = "";
            tokenDic.TryGetValue(key, out accessToken);
            if (key.Contains("_CertJson") && string.IsNullOrEmpty(accessToken))
            {
                accessToken = await _tokenService.GetToken(key);
                tokenDic[key] = accessToken;
            }
            else
            {
                accessToken = key;
            }

            using (var client = new HttpClient())
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                HttpRequestMessage httpRequestMessageClone = Clone(request);
                httpResponseMessage = await client.SendAsync(request);
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    accessToken = await _tokenService.GetToken(key);
                    tokenDic[key] = accessToken;     
                    httpRequestMessageClone.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    httpResponseMessage = await client.SendAsync(httpRequestMessageClone);
                }

                return httpResponseMessage;
            }
        }
        // using System.Reflection;
        // using System.Net.Http;
        // private const string SEND_STATUS_FIELD_NAME = "_sendStatus";
        private void ResetSendStatus(HttpRequestMessage request)
        {
            TypeInfo requestType = request.GetType().GetTypeInfo();
            FieldInfo sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);
            //if (sendStatusField != null)
                //sendStatusField.SetValue(request, 0);
            //else
                //throw new Exception($"Failed to hack HttpRequestMessage, _sendStatus doesn't exist.");
        }
        private HttpRequestMessage Clone(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            if (req.Content != null)
            {
                string json = req.Content.ReadAsStringAsync().Result;
                clone.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
           
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
