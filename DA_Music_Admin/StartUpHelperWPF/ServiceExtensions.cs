using Microsoft.Extensions.DependencyInjection;
using System;

namespace StartUpHelperWPF
{
    public static class ServiceExtensions
    {
        public static void AddWindowFactorySingleton<TForm>(this IServiceCollection services)
            where TForm : class
        {
            services.AddSingleton<TForm>();
            services.AddSingleton<Func<TForm>>(x => ()=> x.GetService<TForm>()!);
            services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
        }
        
        public static void AddWindowFactoryTransient<TForm>(this IServiceCollection services)
            where TForm : class
        {
            services.AddTransient<TForm>();
            services.AddSingleton<Func<TForm>>(x => ()=> x.GetService<TForm>()!);
            services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
        }
    }
}
