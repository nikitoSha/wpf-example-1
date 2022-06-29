using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using wpf_example_1.models;

namespace wpf_example_1.helpers
{
    public class RESTHelper
    {

        public static async Task<HttpResponseMessage> GetCall(string url, Dictionary<string,string> params_ = null)
        {
            string paramsString = "/";
            if (params_ != null && params_.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in params_)
                {
                    paramsString += string.Format("{0}={1}&", kvp.Key, kvp.Value);
                }
            }

            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = GlobalApiUrls.BASE_URL + url + paramsString;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(apiUrl);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Task<HttpResponseMessage> PostCall<T>(string url, T model) where T : class
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = GlobalApiUrls.BASE_URL + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model));

                    var response = client.PostAsync(apiUrl, content);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Task<HttpResponseMessage> PutCall<T>(string url, T model) where T : class
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = GlobalApiUrls.BASE_URL + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model));

                    var response = client.PostAsync(apiUrl, content);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Task<HttpResponseMessage> DeleteCall(string url)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = GlobalApiUrls.BASE_URL + url;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.DeleteAsync(apiUrl);
                    response.Wait();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
