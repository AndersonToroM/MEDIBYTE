using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{
    public partial class EntregaResultadosNoLectura
    {
        [NotMapped]
        public List<long> ListAdmisionesServiciosPrestadosId { get; set; }

        [NotMapped]
        [DDisplayName("EntregaResultadosNoLectura.NombreCompleto")]
        public string NombreCompleto
        {
            get
            {
                return Nombres + " " + Apellidos;
            }
            private set
            {
            }
        }
    }
 }
