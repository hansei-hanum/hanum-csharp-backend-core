using System.Reflection;

namespace Hanum.Core.Helpers;

public static class HanumSwaggerGenServiceCollectionExtensions {
    public static IServiceCollection AddHanumSwaggerGen(this IServiceCollection services) =>
        services.AddSwaggerGen(options => {
            options.IncludeXmlComments(Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
            ));

            options.IncludeXmlComments(Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml"
            ));
        });
}
