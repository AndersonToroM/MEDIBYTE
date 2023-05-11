using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class TemplateModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; } = false;
        public string NameClass { get; set; }
    }
}
