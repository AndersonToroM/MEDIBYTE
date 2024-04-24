using Blazor.Infrastructure.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blazor.BusinessLogic.Models
{
    public class IntegracionConsultarEstadoFEModel : IntegracionFEBase
    {
        public string Status { get; set; }
        public string Cufe {  get; set; }
        public DateTime IssueDate {  get; set; }
        public string DocumentStatus {  get; set; }

    }

    public class IntegracionEnviarFEModel : IntegracionFEBase
    {
        public Guid IdDocumentFE { get; set; }
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
    }

}
