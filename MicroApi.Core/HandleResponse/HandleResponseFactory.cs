using System;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace MicroApi.Core.HandleResponse
{
    public class HandleResponseFactory
    {
        public static IHandleResponse CreateHandleResponse(HttpContext context)
        {
            return (IHandleResponse)context.RequestServices.GetService(Type.GetType($"MicroApi.Core.HandleResponse.IHttp{context.Request.Method.ToLower().Titleize()}HandleResponse"));
        }
    }
}
