using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MicroApi.Core.HandleResponse
{
    public interface IHandleResponse
    {
        HttpContext Context { get; set; }
        object Execute();
        string TableName { get; }
    }
}
