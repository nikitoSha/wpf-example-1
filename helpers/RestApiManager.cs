using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using wpf_example_1.models;
using wpf_example_1.models.posts;

namespace wpf_example_1.helpers
{
    public delegate void RestSuccessEventHandler(object answer);
    public delegate void RestErrorEventHandler(string errMsg);

    public class RestApiManager
    {

        /// <summary>
        /// Получить список постов
        /// </summary>
        /// <param name="restSuccessEvent"></param>
        /// <param name="restErrorEventHandler"></param>
        public static void getPosts(RestSuccessEventHandler restSuccessEvent, RestErrorEventHandler restErrorEventHandler)
        {
            AsyncHelper.doInBackgroundThread(() => {
                Task<HttpResponseMessage> httpResponse = RESTHelper.GetCall("posts");
                    if (httpResponse.Result.IsSuccessStatusCode)
                    {
                        string responsedJson = httpResponse.Result.Content.ReadAsStringAsync().Result;
                        List<PostModel> posts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PostModel>>(responsedJson);
                        restSuccessEvent.Invoke(posts);
                    }
                    else
                    {
                        restErrorEventHandler.Invoke(httpResponse.Result.ReasonPhrase);
                    }
            });
        }

        /// <summary>
        /// Создать пост на сервере
        /// </summary>
        /// <param name="post_"></param>
        /// <param name="restSuccessEvent"></param>
        /// <param name="restErrorEventHandler"></param>
        public static void createPost(
            PostModel post_, 
            RestSuccessEventHandler restSuccessEvent, RestErrorEventHandler restErrorEventHandler)
        {
            AsyncHelper.doInBackgroundThread(() => {
                Task<HttpResponseMessage> httpResponse = RESTHelper.PostCall("posts", post_);
                if (httpResponse.Result.IsSuccessStatusCode)
                {
                    string responsedJson = httpResponse.Result.Content.ReadAsStringAsync().Result;
                    PostModel createdPost = Newtonsoft.Json.JsonConvert.DeserializeObject<PostModel>(responsedJson);
                    restSuccessEvent.Invoke(createdPost);
                }
                else
                {
                    restErrorEventHandler.Invoke(httpResponse.Result.ReasonPhrase);
                }
            });
        }

    }
}
