using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MicroApi.Mapper;

internal class MappingBuilder:IMappingBuilder
{
    private readonly List<TableMapping> _mappings = new List<TableMapping>();

    public IMappingBuilder CreateMap<TFluentEntity>() where TFluentEntity:class,new()
    {
        if (_mappings.Exists(m => m.Source == typeof(TFluentEntity).Name))
            return this;
        var mapping = new TableMapping()
        {
            SourceType = typeof(TFluentEntity),
            Target = GetTableName<TFluentEntity>(true),
            ColumnConfig = GetColumnMappings<TFluentEntity>()
        };
        _mappings.Add(mapping);
        return this;
    }

    public List<TableMapping> Build()
    {
        return _mappings;
    }

    private string GetTableName<T>(bool fluent = false) where T : class, new()
    {
        var tableName = typeof(T).Name;
        if (fluent)
        {
            var tableAttr = typeof(T).GetCustomAttribute(typeof(TableAttribute));
            if (tableAttr is TableAttribute)
            {
                tableName = ((TableAttribute)tableAttr).Name;
            }
        }
        return tableName;
    }

    private Dictionary<string, string> GetColumnMappings<T>() where T : class, new()
    {
        var list = new Dictionary<string, string>();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            list.Add(property.Name, GetColumnName(property, true));
        }
        return list;
    }

    private string GetColumnName(PropertyInfo propertyInfo, bool fluent = false)
    {
        var columnName = propertyInfo.Name;
        if (fluent)
        {
            var columnAttr = propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));
            if (columnAttr is ColumnAttribute)
            {
                columnName = ((ColumnAttribute)columnAttr).Name;
            }
        }
        
        return columnName;
    }
}
