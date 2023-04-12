using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Serialize.Linq.Extensions;
using Dominus.Backend.Data;
using Dominus.Backend.DataBase;

namespace Blazor.Infrastructure.Entities
{
    public partial class Rips
    {
        [NotMapped]
        public bool EsDesdeFactura
        {
            private set { }
            get
            {
                return FacturasId != null;
            }
        }

        [NotMapped]
        public bool EsDesdeEntidad
        {
            private set { }
            get
            {
                return EntidadesId != null;
            }
        }
    }
}
