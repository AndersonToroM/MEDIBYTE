using DevExtreme.AspNet.Data.ResponseModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dominus.Frontend.Data
{
    public class ResultList<T>
    {
        public ResultList()
        {
            TotalCount = -1;
            GroupCount = -1;
        }

        [DefaultValue(-1)]
        [JsonProperty]
        public int TotalCount { get; set; }

        [DefaultValue(-1)]
        [JsonProperty]
        public int GroupCount { get; set; }

        [JsonProperty]
        public object[] Summary { get; set; }

        public List<T> Data { get; set; }

        public List<object> DataObjecs { get; set; }

        public LoadResult ToLoadResult()
        {
            if (Data == null || Data.Count == 0)
                return new LoadResult { data = DataObjecs, groupCount = GroupCount, summary = Summary, totalCount = TotalCount };
            else
                return new LoadResult { data = Data, groupCount = GroupCount, summary = Summary, totalCount = TotalCount };
        }
    }

    public static partial class LoadResultExtention
    {
        public static ResultList<T> ToResultList<T>(this LoadResult result)
        {
            var datas = result.data.OfType<T>();

            return new ResultList<T> { Data = result.data.OfType<T>().ToList(), DataObjecs = result.data.OfType<object>().ToList(), GroupCount = result.groupCount, Summary = result.summary, TotalCount = result.totalCount };
        }
    }
}
