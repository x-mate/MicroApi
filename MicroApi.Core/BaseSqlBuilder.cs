using System.Collections.Generic;
using System.IO;
using MicroApi.Core.Request;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;

namespace MicroApi.Core;

public abstract class BaseSqlBuilder : ISqlBuilder
{
    private readonly IRequest _request;

    protected BaseSqlBuilder(IRequest request)
    {
        _request = request;
    }
    
    public virtual string GetTableName()
    {
        return _httpContext.GetRouteValue("controller").ToString();
    }

    public virtual Dictionary<string, string> GetRequestColumns()
    {
        if (_httpContext.Request.Method == HttpMethods.Get || _httpContext.Request.Method == HttpMethods.Delete)
        {
            return new Dictionary<string, string>();
        }
        //获取body
        var sr = new StreamReader(_httpContext.Request.Body);
        var body = sr.ReadToEndAsync().Result;
        _httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

        var entity = (JObject)JsonConvert.DeserializeObject(body);

        var t = new Dictionary<string, string>();
        foreach (var p in entity.Properties())
        {
            t.Add(p.Name, ((JValue)entity[p.Name])?.Value?.ToString());
        }

        return t;
    }

    public Query BuildQuery()
    {
        return _request.BuildQuery();
    }
}
