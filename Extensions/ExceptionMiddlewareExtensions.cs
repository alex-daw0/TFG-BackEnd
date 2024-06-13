using backend2.CustomExceptionMiddleware;
using backend2.Logger;
using backend2.Modelos;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace backend2.Extensions {
    public static class ExceptionMiddlewareExtensions {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app) {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
