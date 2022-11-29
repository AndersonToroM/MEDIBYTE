using Dominus.Backend.Data;
using Dominus.Backend.DataBase;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Blazor.Infrastructure.Entities
{

    [Table("VReporteCartera")]
    public class VReporteCartera : BaseEntity
    {
        [Column("EntidadId")]
        public long EntidadId { get; set; }

        [Column("Prefijo")]
        public string Prefijo { get; set; }

        [Column("Consecutivo")]
        public long Consecutivo { get; set; }

        [Column("Fecha_Emision")]
        public DateTime? Fecha_Emision { get; set; }

        [Column("Fecha_Radicacion")]
        public string Fecha_Radicacion { get; set; }

        [Column("Plazo")]
        public long? Plazo { get; set; }

        [Column("FechaVencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Column("DiasMora")]
        public int? DiasMora { get; set; }

        [Column("Subtotal_Factura")]
        public decimal Subtotal_Factura { get; set; }

        [Column("Copagos_CuotasModeradoras")]
        public decimal Copagos_CuotasModeradoras { get; set; }

        [Column("Total_Factura")]
        public decimal Total_Factura { get; set; }

        [Column("Porcentaje_ReteFuente")]
        public decimal Porcentaje_ReteFuente { get; set; }

        [Column("Valor_Retefuente")]
        public decimal Valor_Retefuente { get; set; }

        [Column("Porcentaje_ReteICA")]
        public decimal Porcentaje_ReteICA { get; set; }

        [Column("Valor_ReteICA")]
        public decimal Valor_ReteICA { get; set; }

        [Column("Valor_Glosa")]
        public decimal Valor_Glosa{ get; set; }

        [Column("Valor_Aceptado_Glosa")]
        public decimal Valor_Aceptado_Glosa { get; set; }

        [Column("Saldo_Por_Cobrar")]
        public decimal Saldo_Por_Cobrar { get; set; }

        [Column("Pagos_Recibidos")]
        public decimal Pagos_Recibidos { get; set; }

        [Column("Razon_Social_Entidad")]
        public string Razon_Social_Entidad { get; set; }

        [Column("Tipo_Documento")]
        public string Tipo_Documento { get; set; }

        [Column("NumeroIdentificacion")]
        public string NumeroIdentificacion { get; set; }
    }
}
