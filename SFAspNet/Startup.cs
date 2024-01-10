using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFAspNet.Middlewares;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SFAspNet
{
    public class Startup
    {
        public static IWebHostEnvironment env;


        // Метод вызывается средой ASP.NET.
        // Используйте его для подключения сервисов приложения
        // Документация:  https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public Startup(IWebHostEnvironment env)
        {
            Startup.env = env;
        }

        // Метод вызывается средой ASP.NET.
        // Используйте его для настройки конвейера запросов
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Проверяем, не запущен ли проект в среде разработки
            if (env.IsDevelopment())
            {
                // 1. Добавляем компонент, отвечающий за диагностику ошибок
                app.UseDeveloperExceptionPage();
            }

            Console.WriteLine($"Launching project from: {env.ContentRootPath}");

            // Поддержка статических файлов
            app.UseStaticFiles();

            // обрабатываем ошибки HTTP
            app.UseStatusCodePages();

            // 2. Добавляем компонент, отвечающий за маршрутизацию
            app.UseRouting();

            // Подключаем логирвоание с использованием ПО промежуточного слоя
            app.UseMiddleware<LoggingMiddleware>();

            //Добавляем компонент с настройкой маршрутов для главной страницы
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            // Все прочие страницы имеют отдельные обработчики
            app.Map("/about", About);
            app.Map("/config", Config);

/*            // Обработчик для ошибки "страница не найдена"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });*/
        }

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  Обработчик для главной страницы
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
            });
        }
    }
}
