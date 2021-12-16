using Blazor.Infrastructure.Entities;
using System;

namespace Blazor.WebApp.Models
{

    public partial class EntregaResultadosNoLecturaModel
    {
        public EntregaResultadosNoLectura Entity { get; set; }
        public string SerializedResultados { get; set; }
        public DateTime Hora { get; set; }
        public EntregaResultadosNoLecturaModel()
        {
            Entity = new EntregaResultadosNoLectura();
            Entity.ContanciaArchivos = new Archivos();
        }

    }

}
