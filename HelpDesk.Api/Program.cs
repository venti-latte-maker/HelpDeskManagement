
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("TicketConnection");
            builder.Services.AddDbContext<TicketDbContext>(
                options => options.UseSqlServer(connectionString)
                );
            // Add services to the container.

            builder.Services.AddScoped<ITicketRepository, TicketRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
