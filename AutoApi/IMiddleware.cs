using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoApi
{
    public interface IMiddleware
    {
        Task Invoke(HttpContext context);
    }

    public abstract class BaseMiddleware :IMiddleware
    {
        protected readonly RequestDelegate _next;

        public BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public abstract Task Invoke(HttpContext context);
    }
}
