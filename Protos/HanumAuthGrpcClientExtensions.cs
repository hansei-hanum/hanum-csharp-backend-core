
using Grpc.Net.ClientFactory;
using static Hanum.Core.Protos.Authv2.AuthServiceV2;

namespace Hanum.Core.Protos;

public static class HanumAuthGrpcClientExtensions {
    public static IHttpClientBuilder AddHanumAuthGrpcClient(this IServiceCollection services, Action<GrpcClientFactoryOptions> configureClient) =>
        services.AddGrpcClient<AuthServiceV2Client>(configureClient);

    public static IHttpClientBuilder AddHanumAuthGrpcClient(this IServiceCollection services, string connectionString) =>
        services.AddGrpcClient<AuthServiceV2Client>(options => {
            options.Address = new(connectionString);
        });
}