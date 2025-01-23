using ECB.Appilcation.Extentions;
using ECB.Domain.Interfaces.Services;
using ECB.Infrastructure.Configuration;

using ECB.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace ECB;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<ExternalApiOptions>(
            builder.Configuration.GetSection("ExternalApiSettings"));

        builder.Services.AddControllers();
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient();
        builder.Services.AddScoped<ICurrencyRatesService, CurrencyRatesService>();
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        builder.Services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
        });
        
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        
        var app = builder.Build();

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