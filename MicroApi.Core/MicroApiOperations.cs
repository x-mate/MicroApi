using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroApi.Core
{
    public static class MicroApiOperations
    {
        public static OperationAuthorizationRequirement GetOperation(string method)
        {
            return new OperationAuthorizationRequirement { Name = method };
        }
    }
}
