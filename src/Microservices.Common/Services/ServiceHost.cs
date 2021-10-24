using System;
using Microservices.Common.Commands;
using Microservices.Common.Events;
using Microservices.Common.RabbitMq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RawRabbit;

namespace Microservices.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run() => _webHost.Run();

        public static HostBuilder Create<T>(string[] args) where T : class
        {
            Console.Title = typeof(T).Namespace;
            var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder()
            .UseConfiguration(config)
            .UseStartup<T>();

            return new HostBuilder(webHostBuilder.Build());

        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {

            private readonly IWebHost _webHost;
            private IBusClient _bus;

            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public BusBuilder UseRabbitmq()
            {
                _bus = (IBusClient)_webHost.Services.GetService(typeof(IBusClient));
                return new BusBuilder(_webHost, _bus);
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _bus;

            public BusBuilder(IWebHost webHost, IBusClient bus)
            {
                _webHost = webHost;
                _bus = bus;
            }

            public BusBuilder SubscribeToCommand<T>() where T : ICommand
            {
                var handler = (ICommandHandler<T>)_webHost.Services
                .GetService(typeof(ICommandHandler<T>));
                _bus.WithCommandHandlerAsync(handler);
                return this;
            }

            public BusBuilder SubscribeToEvent<T>() where T : IEvent
            {
                var handler = (IEventHandler<T>)_webHost.Services
                .GetService(typeof(IEventHandler<T>));
                _bus.WithEventHandlerAsync(handler);
                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }
    }
}