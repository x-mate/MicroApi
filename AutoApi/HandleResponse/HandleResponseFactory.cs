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
            //var type = Assembly.GetExecutingAssembly().GetType($"AutoApi.HandleResponse.Http{context.Request.Method.ToLower().Titleize()}HandleReponse");
            return (IHandleResponse)context.RequestServices.GetService(Type.GetType($"AutoApi.Core.HandleResponse.IHttp{context.Request.Method.ToLower().Titleize()}HandleResponse"));
            //return (IHandleResponse)Activator.CreateInstance(
            //    Type.GetType($"AutoApi.Core.HandleResponse.Http{context.Request.Method.ToLower().Titleize()}HandleResponse"), context);
        }
    }
}
