namespace Blazor.Infrastructure.Entities
{
    public partial class Admisiones
    {
        #region Referencia de salida

        public virtual Atenciones Atenciones { get; set; }

        #endregion

    }
}
