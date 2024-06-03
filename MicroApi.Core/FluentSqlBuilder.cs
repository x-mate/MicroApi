using MicroApi.Core;
using MicroApi.Core.Request;
using Microsoft.AspNetCore.Http;

namespace MicroApi.Mapper;

public class FluentSqlBuilder:BaseSqlBuilder
{
    private readonly IMappingBuilder _mappingBuilder;
    public FluentSqlBuilder(IRequest request, IMappingBuilder mappingBuilder) : base(request)
    {
        _mappingBuilder = mappingBuilder;
    }

    public override string GetTableName()
    {
        var requestName = base.GetTableName();
        return GetTableMapping()?.Source ?? requestName;
    }

    private TableMapping? GetTableMapping()
    {
        return _mappingBuilder.Build().FirstOrDefault(m =>
            m.Target.Equals(base.GetTableName(), StringComparison.OrdinalIgnoreCase));
    }

    public override Dictionary<string, string> GetRequestColumns()
    {
        var columns = base.GetRequestColumns();
        var tableMapping = GetTableMapping();
        if (!columns.Any() || !(tableMapping?.ColumnConfig?.Any() ?? false))
        {
            return columns;
        }

        var result = new Dictionary<string, string>();
        foreach (var item in columns)
        {
            var dbColumnName = tableMapping.ColumnConfig.FirstOrDefault(c =>
                c.Key.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                .Value;
            result.Add(item.Key, String.IsNullOrEmpty(dbColumnName)? item.Value: dbColumnName);
        }

        return result;
    }
}
