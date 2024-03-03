using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hanum.Core.Helpers;

public static class HanumDbContextServiceCollectionExtensions {
    public static IServiceCollection AddHanumDbContexts<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors |
         DynamicallyAccessedMemberTypes.PublicProperties)] TContext>(this IServiceCollection serviceCollection, string? connectionString, IConfigurationSection? databaseOptions = null) where TContext : DbContext {
        void DbOptionsBuilder(DbContextOptionsBuilder options) {
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: databaseOptions?.GetValue<int?>("MaxRetryCount", null) ?? 5,
                    maxRetryDelay: TimeSpan.FromSeconds(databaseOptions?.GetValue<int?>("MaxRetryDelay", null) ?? 30),
                    errorNumbersToAdd: null
                )
            );
        }

        serviceCollection.AddDbContextPool<TContext>(DbOptionsBuilder, databaseOptions?.GetValue<int?>("MaxPoolSize", null) ?? 5);
        serviceCollection.AddDbContextFactory<TContext>(DbOptionsBuilder);

        return serviceCollection;
    }
}