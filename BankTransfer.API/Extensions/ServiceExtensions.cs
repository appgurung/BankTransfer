using BankTransfer.API.Providers.Paystack;
using BankTransfer.API.Services;
using BankTransfer.Core.Helpers;
using BankTransfer.Core.Interface;
using BankTransfer.Infrastructure.Context;
using BankTransfer.Infrastructure.Interface;
using BankTransfer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace BankTransfer.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCBAServices(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration Configuration = builder.Build();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<ICoreBanking, CoreBankingService>();
            services.AddScoped<ConfigHelper>();
            services.AddTransient<IRestClientHelper, RestClientHelper>();
            services.AddTransient<IPaystackProvider, PaystackProviderService>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryService<>));
            services.AddDbContext<CoreBankingContext>(item => item.UseSqlServer(Configuration.GetConnectionString("BillerDBConnection")!, x => x.MigrationsAssembly("BankTransfer.API")));
            return services;
        }

        public static IApplicationBuilder UseCBAMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;

        }
    }
}
