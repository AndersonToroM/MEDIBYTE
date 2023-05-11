using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class IndexModel
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string ColumnCode { get; set; }
        public string TableName { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

}
