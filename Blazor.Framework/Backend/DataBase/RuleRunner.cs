using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Dominus.Backend.DataBase
{
    public class RuleRunner
    {
        public bool IsTrue<T>(string rule, T data) where T : BaseEntity
        {
            string exp = @rule;
            var p = Expression.Parameter(typeof(T), "x");
            var e = DynamicExpressionParser.ParseLambda(new[] { p }, null, exp);
            var r = e.Compile().DynamicInvoke(data);
            return r != null ? bool.Parse(r.ToString()) : false;
        }

        
        public T SetCalculationRules<T>(string rule, T data) where T : BaseEntity
        {
            List<T> datas = new List<T>();
            datas.Add(data);
            return datas.AsQueryable().Select(rule).ToDynamicList().FirstOrDefault();

        }

    }

}
