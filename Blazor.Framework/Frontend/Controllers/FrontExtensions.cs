using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Dominus.Frontend.Controllers
{
    public static class FrontExtensions
    {

        public static string GetModelFullErrorMessage(this ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return string.Join(" ", messages);
        }

        public static string GetFrontFullErrorMessage(this Exception exeption)
        {
            var messages = new List<string>();
            while (exeption != null)
            {
                messages.Add(exeption.Message);
                exeption = exeption.InnerException;
            }
            return string.Join(" ", messages);
        }
    }

}
