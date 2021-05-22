﻿using System;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Http;

namespace AutoApi.HandleResponse
{
    public class HandleResponseFactory
    {
        public static IHandleResponse CreateHandleResponse(HttpContext context)
        {
            //var type = Assembly.GetExecutingAssembly().GetType($"AutoApi.HandleResponse.Http{context.Request.Method.ToLower().Titleize()}HandleReponse");
            return (IHandleResponse)Activator.CreateInstance(
                Type.GetType($"AutoApi.HandleResponse.Http{context.Request.Method.ToLower().Titleize()}HandleResponse"), context);
        }
    }
}
