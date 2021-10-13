using System;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace AutoApi.Core.HandleResponse
{
    public class HandleResponseFactory
    {
        public static IHandleResponse CreateHandleResponse(HttpContext context)
        {
            return (IHandleResponse)context.RequestServices.GetService(Type.GetType($"AutoApi.Core.HandleResponse.IHttp{context.Request.Method.ToLower().Titleize()}HandleResponse"));
        }
    }
}
