using Microsoft.Extensions.DependencyInjection;

namespace MicroApi.Mapper;

public interface IMappingBuilder
{
    IMappingBuilder CreateMap<TFluentEntity>() where TFluentEntity : class, new();

    List<TableMapping> Build();
}


public static class MappingBuilderExtensions
{
    public static IServiceCollection AddFluentMapping(this IServiceCollection collection, Action<IMappingBuilder> config)
    {
        IMappingBuilder builder = new MappingBuilder();
        config?.Invoke(builder);
        return collection.AddSingleton(builder);
    }
}
