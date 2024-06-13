using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dominus.Backend.Application
{
    public static class Extensions
    {

        public static string GetFullErrorMessage(this Exception exeption)
        {
            var messages = new List<string>();
            while (exeption != null)
            {
                messages.Add(exeption.Message);
                exeption = exeption.InnerException;
            }
            return String.Join(" ", messages);
        }

        public static string GetModelFullError(this ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

    }
}


