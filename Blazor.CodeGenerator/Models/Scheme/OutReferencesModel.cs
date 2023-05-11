using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class OutReferencesModel
    {
        public string OutReferencesName { get; set; }
        public string TableName { get; set; }
        public string ParentTableName { get; set; }
        public string ParentTableCode { get; set; }
        public string ColumnName { get; set; }
        public string ColumnCode { get; set; }
        public string ParentColumnName { get; set; }
        public string ParentColumnCode { get; set; }
        //public List<ColumnModel> ParentColumns { get; set; }
    }

}
