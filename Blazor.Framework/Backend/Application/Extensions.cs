﻿using System.Collections.Generic;
using System;

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

    }
}

