using Blazor.Infrastructure.Entities.Models;
using Dominus.Backend.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.BusinessLogic.Models
{
    public class IntegracionXmlFEModel : IntegracionFEBase
    {
        public string FileName { get; set; }
        public string ContentBase64 { get; set; }
        public string PathFile { get; set; }
        public string ContentType
        {
            get
            {
                return DApp.Util.ObtenerContentTypePorExtension(FileName);
            }
        }

        public byte[] ContentBytes
        {
            get
            {
                return Convert.FromBase64String(ContentBase64);
            }
        }
    }

    public class IntegracionEnviarFEModel : IntegracionFEBase
    {
        public Guid? IdDocumentFE { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Cufe { get; set; }
        public DateTime? IssueDate { get; set; }
    }

    public class IntegracionSeriesFEModel : IntegracionFEBase
    {
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public string AuthorizationNumber { get; set; }
        public string TechnicalKey { get; set; }
        public string ExternalKey { get; set; }

        public List<FEResultadoSeries> ResultadoSeries { get; set; } = new List<FEResultadoSeries>();
    }

    public class IntegracionFEBase
    {
        public bool HuboErrorIntegracion { get; set; }
        public bool HuboErrorFE { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
        public int? HttpStatus { get; set; }
        public string JsonResult { get; set; }
        public string Api { get; set; } = string.Empty;
    }

}
