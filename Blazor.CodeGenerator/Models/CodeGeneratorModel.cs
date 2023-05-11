using CodeGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public class CodeGeneratorModel
    {
        public List<DBSettings> Conexiones { get; set; }
        public int ConexionActual { get; set; }
        public List<Framework> Frameworks { get; set; } = new List<Framework>
        {
            new Framework{ Id = 1, Descripcion= ".Net Core" }
        };
        public int FrameworkActual { get; set; }
        public List<TableModel> Tables { get; set; }
        public List<TemplateModel> Templates { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
        public string Domain { get; set; }
        public string NameSpace { get; set; }
        public string PathGenerate { get; set; } = GCUtil.PathTemp;
        public TableModel TableGenerated { get; set; }
        public TemplateModel TemplateGenerated { get; set; }
        public DataBaseMapModel DataBaseMapModel { get; set; } = new DataBaseMapModel();

        public List<TipoOrigen> TipoOrigen { get; set; } = new List<TipoOrigen>
        {
            new TipoOrigen{ Id = 1, Descripcion= "Tabla" }, new TipoOrigen{ Id = 2, Descripcion = "Roe" }
        };
        public int TipoOrigenActual { get; set; }

        public List<string> ProyectoArea { get; set; } = new List<string>();


        public string ConnectionId { get; set; } 
    }

    public class Framework {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class TipoOrigen
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

}
