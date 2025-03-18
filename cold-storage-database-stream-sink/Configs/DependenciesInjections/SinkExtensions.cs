using cold_storage_database_stream_sink.src.Configs.Options;
using Microsoft.Extensions.Options;

namespace cold_storage_database_stream_sink.src.Configs.DependenciesInjections
{
    public static class SinkExtensions
    {
        public static IServiceCollection AddSinkExtension(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<SinkOptions>(opt =>
            {
                opt.DatabaseHost = configuration.GetValue<string>("DATABASE_HOST");
                opt.DatabaseListeningEvent = configuration.GetValue<string>("DATABASE_LISTENING_EVENT");
                opt.ProviderStreamRegion = configuration.GetValue<string>("PROVIDER_STREAM_REGION");
                opt.ProviderSecretId = configuration.GetValue<string>("PROVIDER_SECRET_ID");
                opt.ProviderSecretKey = configuration.GetValue<string>("PROVIDER_SECRET_KEY");
                opt.StreamHost = configuration.GetValue<string>("STREAM_HOST");
                opt.StreamName = configuration.GetValue<string>("STREAM_NAME");
            });

            services.AddSingleton<SinkOptions>(sp =>
                    sp.GetRequiredService<IOptions<SinkOptions>>().Value);

            return services;
        }
    }
}
