using MassTransit;
using System.Reflection;
using TimedEvents;
using TimedEventWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var ServiceConfiguration = new ServiceConfiguration();
        hostContext.Configuration.GetSection(ServiceConfiguration.ConfigurationKey).Bind(ServiceConfiguration);
        services.AddSingleton(ServiceConfiguration);

        services.AddMassTransit(configuration =>
        {
            configuration.SetKebabCaseEndpointNameFormatter();

            var curentAssembly = Assembly.GetEntryAssembly();
            configuration.AddConsumers(curentAssembly);

            configuration.UsingRabbitMq((busConfiguration, busFactoryConfiguration) =>
            {
                busFactoryConfiguration.Host(ServiceConfiguration.Host, ServiceConfiguration.Port, ServiceConfiguration.VirtualHost, rHostConfig =>
                {
                    rHostConfig.Username(ServiceConfiguration.User);
                    rHostConfig.Password(ServiceConfiguration.Password);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();