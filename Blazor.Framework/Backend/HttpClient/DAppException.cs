using Dominus.Backend.Application;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Dominus.Backend.HttpClient
{
    public class DAppException : Exception
    {
        public List<string> Errors { get; set; }

        public DAppException(string error)
        {
            Errors = new List<string> { error };
        }

        public DAppException(List<string> errors)
        {
            Errors = errors;
        }
    }

    public static class ExceptionHandlerExtension
    {
        public static void UseDAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    Exception ex = context.Features.Get<IExceptionHandlerFeature>().Error;

                    DApp.LogToFile($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} | {nameof(ExceptionHandlerExtension)}.UseDAppExceptionHandler() | {ex.GetFullErrorMessage()} | {ex.StackTrace}");
                    
                    if (ex is DAppException)
                    {
                        var appEx = ex as DAppException;
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json charset=utf-8";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(appEx.Errors, Formatting.Indented), Encoding.UTF8);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain charset=utf-8";
                        await context.Response.WriteAsync(ex.GetFullErrorMessage(), Encoding.UTF8);
                    }
                });
            });
        }
    }
}
