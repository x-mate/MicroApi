using MicroApi.Core.Request;
using Microsoft.AspNetCore.Http;

namespace MicroApi.Core;

internal class DefaultSqlBuilder : BaseSqlBuilder
{
    public DefaultSqlBuilder(IRequest request) : base(request)
    {
    }
}
