using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Blazor.Infrastructure.Entities
{

    [Table("VExportarSiigo")]
    public class VExportarSiigo : BaseEntity
    {
        [Column("Prefijo")]
        public string Prefijo { get; set; }

        [Column("Consecutivo")]
        public long Consecutivo { get; set; }

        [Column("Tipo_Documento")]
        public string Tipo_Documento { get; set; }

        [Column("Numero_Identificacion")]
        public string Numero_Identificacion { get; set; }

        [Column("Razon_Social")]
        public string Razon_Social{ get; set; }

        [Column("Nombres_Tercero")]
        public string Nombres_Tercero { get; set; }

        [Column("Apellidos_Tercero")]
        public string Apellidos_Tercero { get; set; }

        [Column("Nit_Entidad_Copago")]
        public string Nit_Entidad_Copago { get; set; }

        [Column("Fecha_Emision")]
        public DateTime? Fecha_Emision { get; set; }

        [Column("Email_Contacto")]
        public string Email_Contacto { get; set; }

        [Column("Codigo_Producto")]
        public string Codigo_Producto { get; set; }

        [Column("Descripcion_Producto")]
        public string Descripcion_Producto { get; set; }

        [Column("Cantidad_Producto")]
        public int? Cantidad_Producto { get; set; }

        [Column("Valor_Unitario")]
        public int? Valor_Unitario { get; set; }

        [Column("Valor_Descuento")]
        public int? Valor_Descuento { get; set; }

        [Column("Valor_Copago")]
        public int? Valor_Copago { get; set; }

        [Column("Subtotal_Factura")]
        public int? Subtotal_Factura { get; set; }

        [Column("FechaVencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Column("Observaciones")]
        public string Observaciones { get; set; }

    }
}