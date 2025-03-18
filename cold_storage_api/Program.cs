
using cold_storage_api.Services;
using cold_storage_api.Services.Interfaces;
using Serilog;

namespace cold_storage_api
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
            Serilog.Core.Logger logger = new LoggerConfiguration()
          .ReadFrom.Configuration(builder.Configuration)
          .Enrich.FromLogContext()
          .CreateLogger();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSerilog(logger);
            builder.Services.AddSingleton<CSVService>();
            builder.Services.AddSingleton<ColdStorageService>();
            builder.Services.AddTransient<IStreamConsumer, KinesisStreamConsumer>();
            builder.Services.AddTransient<IFileStorageService, FileStorageService>();
            builder.Services.AddTransient<IDataCatalogService, DataCatalogService>();
        

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
