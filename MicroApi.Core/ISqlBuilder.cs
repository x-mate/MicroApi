using System.Collections.Generic;
using SqlKata;

namespace MicroApi.Core;

public interface ISqlBuilder
{
    // string GetTableName();
    //
    // Dictionary<string, string> GetRequestColumns();
    
    Query BuildQuery();
}
