using System.Collections.Generic;
using System.ComponentModel;

namespace Blazor.BusinessLogic.Models
{
    public class ArchivoDescargaModel
    {
        public string Nombre { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public byte[] Archivo { get; set; }
    }
}
