using Blazor.Infrastructure.Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blazor.BusinessLogic.Models
{
    public class IntegracionEnviarFEModel
    {
        public bool HuboErrorIntegracion { get; set; }
        public bool HuboErrorFE { get; set; }
        public string ErrorIntegration { get; set; }
        public List<string> ErroresRespuesta { get; set; }
        public int? HttpStatus { get; set; }
        public string JsonResult { get; set; }
        public Guid IdDocumentFE { get; set; }
    }

    public class IntegracionSeriesFEModel
    {
        public bool HuboErrorIntegracion { get; set; }
        public bool HuboErrorFE { get; set; }
        public string ErrorIntegracion { get; set; }
        public List<string> ErroresRespuesta { get; set; }
        public int? HttpStatus { get; set; }
        public string JsonResult { get; set; }

        public string TechnicalKey { get; set; }
        public string ExternalKey { get; set; }

        public List<FEResultadoSeries> ResultadoSeries { get; set; }
    }

}
