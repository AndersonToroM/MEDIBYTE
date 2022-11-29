using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dominus.Backend.HttpClient
{
    public static class HttpRequestFactory
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                JsonSerializerSettings options = new JsonSerializerSettings();
                options.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.NullValueHandling = NullValueHandling.Ignore;
                options.TypeNameHandling = TypeNameHandling.Objects;
                options.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                options.ContractResolver = new DefaultContractResolver();
                options.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fff";
                return options;
            }
        }

        //public static JsonSerializerOptions Settings
        //{
        //    get { return new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = null }; }
        //}

        public static async Task<HttpResponseMessage> Get(string requestUri)
            => await Get(requestUri, "");

        public static async Task<HttpResponseMessage> Get(string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);
            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Get(string requestUri, object value)
            => await Get(requestUri, "");

        public static async Task<HttpResponseMessage> Get(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value,HttpRequestFactory.Settings))
                                .AddBearerToken(bearerToken);
            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, object value)
            => await Post(requestUri, value, "");

        public static async Task<HttpResponseMessage> Post(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value, HttpRequestFactory.Settings))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        //public static async Task<HttpResponseMessage> PostExpression(string requestUri, object value, string bearerToken)
        //{
        //    var builder = new HttpRequestBuilder()
        //                        .AddMethod(HttpMethod.Post)
        //                        .AddRequestUri(requestUri)
        //                        .AddContent(new JsonContent(value, new JsonSerializerOptions()))
        //                        .AddBearerToken(bearerToken);

        //    return await builder.SendAsync();
        //}

        public static async Task<HttpResponseMessage> PostExpression(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        //public static async Task<HttpResponseMessage> PostExpression(string requestUri, object value, string bearerToken, JsonSerializerOptions settings)
        //{
        //    var builder = new HttpRequestBuilder()
        //                        .AddMethod(HttpMethod.Post)
        //                        .AddRequestUri(requestUri)
        //                        .AddContent(new JsonContent(value, settings))
        //                        .AddBearerToken(bearerToken);

        //    return await builder.SendAsync();
        //}

        public static async Task<HttpResponseMessage> PostExpression(string requestUri, object value, string bearerToken, JsonSerializerSettings settings)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value, settings))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, object value)
            => await Put(requestUri, value, "");

        public static async Task<HttpResponseMessage> Put(
            string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value, HttpRequestFactory.Settings))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(string requestUri, object value)
            => await Patch(requestUri, value, "");

        public static async Task<HttpResponseMessage> Patch(
            string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri)
                                .AddContent(new PatchContent(value))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri)
            => await Delete(requestUri, "");

        public static async Task<HttpResponseMessage> Delete( string requestUri, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri)
                                  .AddContent(new JsonContent(value, HttpRequestFactory.Settings))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName)
            => await PostFile(requestUri, filePath, apiParamName, "");

        public static async Task<HttpResponseMessage> PostFile(string requestUri,
            string filePath, string apiParamName, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new FileContent(filePath, apiParamName))
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }

        //public static async Task<HttpResponseMessage> PostDataSourceLoadOptions(string requestUri, object value, string bearerToken)
        //{
        //    var builder = new HttpRequestBuilder()
        //                        .AddMethod(HttpMethod.Post)
        //                        .AddRequestUri(requestUri)
        //                        //.AddAcceptHeader("text/javascript")
        //                        .AddBearerToken(bearerToken)
        //                        .AddContent(new JsonContent(value, new JsonSerializerOptions()));
        //                        //.AddContent(new JsonContent(value));
        //    return await builder.SendAsync();
        //}

        public static async Task<HttpResponseMessage> PostDataSourceLoadOptions(string requestUri, object value, string bearerToken)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                //.AddAcceptHeader("text/javascript")
                                .AddBearerToken(bearerToken)
                                .AddContent(new JsonContent(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
            //.AddContent(new JsonContent(value));
            return await builder.SendAsync();
        }



        //public static async Task<HttpResponseMessage> PostDataSourceLoadOptions(string requestUri, object value, string bearerToken, JsonSerializerOptions settings)
        //{
        //    var builder = new HttpRequestBuilder()
        //                        .AddMethod(HttpMethod.Post)
        //                        .AddRequestUri(requestUri)
        //                        .AddContent(new JsonContent(value, settings))
        //                        //.AddAcceptHeader("text/javascript")
        //                        .AddBearerToken(bearerToken);

        //    return await builder.SendAsync();
        //}

        public static async Task<HttpResponseMessage> PostDataSourceLoadOptions(string requestUri, object value, string bearerToken, JsonSerializerSettings settings)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri)
                                .AddContent(new JsonContent(value, settings))
                                //.AddAcceptHeader("text/javascript")
                                .AddBearerToken(bearerToken);

            return await builder.SendAsync();
        }
    }
}
