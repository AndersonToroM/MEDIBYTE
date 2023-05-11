using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class ColumnModel
    {
        public string Name { get; set; }
        public string Default { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int Quantity { get; set; }
        public int Decimals { get; set; }
        public string CodeJava { get; set; }
        public string Code { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsIndexUnique { get; set; }
        public bool IsFKIn { get; set; }
        public bool IsFKOut { get; set; }
        public bool IsRequired { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsInteger { get; set; }
        public bool IsDecimal{ get; set; }
        public string TableName{ get; set; }
        public string TableCode{ get; set; }
        public string Number { get; set; }
    }

}
