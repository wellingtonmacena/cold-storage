using cold_storage_database_stream_sink.src.Configs.DependenciesInjections;
using cold_storage_database_stream_sink.src.Services;
using Serilog;
using System.Text.Json;

namespace ColdStorage_DataPlane.src
{
    public class Program
    {
        public static void Main(string[] args)
        {

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();

            // Add services to the container.

            Console.WriteLine(JsonSerializer.Serialize($" ENVIRONMENT: {builder.Configuration.GetValue<string>("ENVIRONMENT")} -- {builder.Configuration.AsEnumerable()}"));

            Serilog.Core.Logger logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()// L� a configura��o do appsettings.json
            .CreateLogger();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHostedService<KeyValueDbListenerService>();

             builder.Services.AddSerilog(logger);
            builder.Services.AddSinkExtension(builder.Configuration);
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
