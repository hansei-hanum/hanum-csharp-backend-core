
using Grpc.Net.ClientFactory;
using static Hanum.Core.Protos.Auth.AuthService;

namespace Hanum.Core.Protos;

public static class HanumAuthGrpcClientExtensions {
    public static IHttpClientBuilder AddHanumAuthGrpcClient(this IServiceCollection services, Action<GrpcClientFactoryOptions> configureClient) =>
        services.AddGrpcClient<AuthServiceClient>(configureClient);

    public static IHttpClientBuilder AddHanumAuthGrpcClient(this IServiceCollection services, string connectionString) =>
        services.AddGrpcClient<AuthServiceClient>(options => {
            options.Address = new(connectionString);
        });
}