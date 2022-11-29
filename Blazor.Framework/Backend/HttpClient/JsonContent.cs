using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Dominus.Backend.HttpClient
{
    //public class JsonContent : StringContent
    //{
    //    public JsonContent(object value)
    //        : base(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
    //    {
    //    }

    //    public JsonContent(object value, string mediaType)
    //        : base(JsonSerializer.Serialize(value), Encoding.UTF8, mediaType)
    //    {
    //    }

    //    public JsonContent(object value, JsonSerializerOptions settings)
    //        : base(JsonSerializer.Serialize(value, settings), Encoding.UTF8, "application/json")
    //    {
    //    }

    //}
    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(object value, string mediaType)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
        {
        }

        public JsonContent(object value, JsonSerializerSettings settings)
            : base(JsonConvert.SerializeObject(value, settings), Encoding.UTF8, "application/json")
        {
        }

        //public JsonContent(object value, string mediaType)
        //    : base(JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }), Encoding.UTF8, mediaType)
        //{
        //}
    }

}
