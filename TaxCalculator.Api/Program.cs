using TaxCalculator.Api.Configuration;
using TaxCalculator.Core.Configuration;
using TaxCalculator.Core.Configuration.Models;
using TaxCalculator.Core.Domain.Interfaces;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Data;

namespace TaxCalculator.Api;
public class Program
{
    private Program()
    {
    }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<TaxConfig>(builder.Configuration.GetSection("TaxConfig"));
        builder.Services.AddScoped<ITaxConfiguration, TaxConfiguration>();
        builder.Services.AddScoped<ITaxPayerRepository, TaxPayerRepository>();
        builder.Services.AddScoped<ITaxCalculator, TaxCalculator.Core.Domain.TaxCalculator>();

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
