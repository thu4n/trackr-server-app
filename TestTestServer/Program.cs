using Microsoft.EntityFrameworkCore;
using TestTestServer.Data;
using MySqlConnector;
using System.Runtime.Intrinsics;
using Microsoft.AspNetCore.Mvc;
namespace TestTestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddConnections();
            // Thêm các service là các class không nằm bên trong các folder
            builder.Services.AddSingleton<FileService>(); // cho phần up file
            builder.Services.AddSingleton<EsistAccountService>(); // cho phần check Account 
            builder.Services.AddSingleton<ProcessTree>();   // cho phần cây tiến trình
            builder.Services.AddDbContext<APIData>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ApiDatabase")));
          //  builder.Services.AddDbContext<APIData>(options => options.UseInMemoryDatabase("ApiDatabase"));
            
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
}