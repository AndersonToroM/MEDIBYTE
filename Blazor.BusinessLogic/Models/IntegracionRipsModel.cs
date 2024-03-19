using Blazor.Infrastructure.Entities.Models;
using DevExpress.Office.Utils;
using System.Collections.Generic;

namespace Blazor.BusinessLogic.Models
{
    public class IntegracionRipsModel
    {
        public bool HuboErrorIntegracion { get; set; }
        public bool HuboErrorRips { get; set; }
        public string Error { get; set; }
        public int? HttpStatus { get; set; }
        public RespuestaIntegracionRips Respuesta { get; set;}
    }
}
