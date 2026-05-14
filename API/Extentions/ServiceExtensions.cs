using Application.Common.Behaviors;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contaxt;
using Persistence.Repositories;

namespace API.Extentions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Products.Commands.Create.Command).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(Application.Products.Commands.Create.Command).Assembly);
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
