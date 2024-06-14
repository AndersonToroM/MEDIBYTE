using System.Collections.Generic;
using System;

namespace Dominus.Backend.Application
{
    public static class BackExtension
    {
        public static string GetBackFullErrorMessage(this Exception exeption)
        {
            var messages = new List<string>();
            while (exeption != null)
            {
                messages.Add(exeption.Message);
                exeption = exeption.InnerException;
            }
            return String.Join(" ", messages);
        }
    }

    public enum TypeLog
    {
        Error = 1,
        Informativo = 2,
        Advertencia = 3
    }
}
