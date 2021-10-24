using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.Commands;
using RawRabbit;
using RawRabbit.Pipe;
using Microservices.Common.Events;

namespace Microservices.Common.RabbitMq
{
    public static class Extensions
    {
        public static Task WithCommandHandlerAsync<T>(this IBusClient bus,
        ICommandHandler<T> handler) where T : ICommand => bus.SubscribeAsync<T>(
            msg => handler.HandleAsync(msg),
            ctx => ctx.UseConsumerConfiguration(
                cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<T>()))
            )
        );

        public static Task WithEventHandlerAsync<T>(this IBusClient bus,
        IEventHandler<T> handler) where T : IEvent => bus.SubscribeAsync<T>(
            msg => handler.HandleAsync(msg),
            ctx => ctx.UseConsumerConfiguration(
                cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<T>()))
            )
        );

        private static string GetQueueName<T>() => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";
    }
}