using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace AutoApi
{
    public class AutoRestfulApiOption
    {
        public string ApiRoutePrefix { get; set; } = "/api";
    }
}
