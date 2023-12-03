
using EmailSender.Interfaces;
using EmailSender.Models;
using EmailSender.Services;

namespace EmailSender
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
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>(); // objetos son siempre los mismos para cada objeto y cada solicitud
            
            //builder.Services.AddScoped<IEmailService,DotNetEmailService>(); // objetos son los mismos dentro de una solicitud, pero diferentes en diferentes solicitudes
            //builder.Services.AddTransient<IEmailService, DotNetEmailService>(); // objectos son siempre diferentes, siempre se provee una nueva instancia para cada controlador y cada servicio

            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(nameof(EmailConfiguration)));

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
