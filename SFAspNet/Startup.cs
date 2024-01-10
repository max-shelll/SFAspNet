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


        // ����� ���������� ������ ASP.NET.
        // ����������� ��� ��� ����������� �������� ����������
        // ������������:  https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public Startup(IWebHostEnvironment env)
        {
            Startup.env = env;
        }

        // ����� ���������� ������ ASP.NET.
        // ����������� ��� ��� ��������� ��������� ��������
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ���������, �� ������� �� ������ � ����� ����������
            if (env.IsDevelopment())
            {
                // 1. ��������� ���������, ���������� �� ����������� ������
                app.UseDeveloperExceptionPage();
            }

            Console.WriteLine($"Launching project from: {env.ContentRootPath}");

            // ��������� ����������� ������
            app.UseStaticFiles();

            // ������������ ������ HTTP
            app.UseStatusCodePages();

            // 2. ��������� ���������, ���������� �� �������������
            app.UseRouting();

            // ���������� ����������� � �������������� �� �������������� ����
            app.UseMiddleware<LoggingMiddleware>();

            //��������� ��������� � ���������� ��������� ��� ������� ��������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            // ��� ������ �������� ����� ��������� �����������
            app.Map("/about", About);
            app.Map("/config", Config);

/*            // ���������� ��� ������ "�������� �� �������"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });*/
        }

        /// <summary>
        ///  ���������� ��� �������� About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  ���������� ��� ������� ��������
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
