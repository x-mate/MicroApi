using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoApi.HandleResponse
{
    public interface IHandleResponse
    {
        public HttpContext Context { get; set; }
        public string Execute();
    }
}
