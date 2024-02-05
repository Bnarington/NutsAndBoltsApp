using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace Presentation;


public partial class App : Application
{
    private static IHost? builder;

    public App()
    {
        builder = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Skoldokument\Datalagring\NutsAndBoltsApp\Infrastructure\Data\local_db.mdf;Integrated Security=True;Connect Timeout=30"));
                //services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddScoped<BoltRepository>();
                services.AddScoped<NutRepository>();
                services.AddScoped<ProductRepository>();
                services.AddScoped<RoleRepository>();
                services.AddScoped<UserRepository>();
                services.AddScoped<ProductService>();
                services.AddScoped<UserService>();
                services.AddScoped<RoleService>();
            }).Build();
    }


    protected override void OnStartup(StartupEventArgs e)
    {

        builder!.Start();

        var mainWindow = builder!.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}
