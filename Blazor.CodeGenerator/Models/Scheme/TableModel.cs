using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class TableModel
    {
        public string Name { get; set; }
        public bool Selected { get; set; } = false;
        public string Code { get; set; }
        public string Comment { get; set; }
        public string NameProject { get; set; }
        public string Prefix { get; set; }
        public string Number { get; set; }
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
        public List<InReferencesModel> InReferences { get; set; } = new List<InReferencesModel>();
        public List<OutReferencesModel> OutReferences { get; set; } = new List<OutReferencesModel>();
        public List<IndexModel> Indexes { get; set; } = new List<IndexModel>();
    }

}
