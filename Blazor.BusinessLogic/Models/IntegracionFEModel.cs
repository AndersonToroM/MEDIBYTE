﻿namespace Blazor.BusinessLogic.Models
{
    public class IntegracionFEModel
    {
        public bool HuboErrorIntegracion { get; set; }
        public bool HuboErrorFE { get; set; }
        public string Error { get; set; }
        public int? HttpStatus { get; set; }
        public string JsonResult { get; set; }
    }
}