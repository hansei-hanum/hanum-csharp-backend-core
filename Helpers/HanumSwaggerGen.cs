using System.Reflection;

namespace Hanum.Core.Helpers;

public static class HanumSwaggerGenServiceCollectionExtensions {
    public static IServiceCollection AddHanumSwaggerGen(this IServiceCollection services) =>
        services.AddSwaggerGen(options => {
            void IncludeXmlComments(string path) {
                if (File.Exists(path))
                    options.IncludeXmlComments(path);
            }

            IncludeXmlComments(Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
            ));

            IncludeXmlComments(Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml"
            ));
        });
}
