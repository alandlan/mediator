

using Mediator.Core.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Core.Implementation
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public async Task Publish<TNotification>(TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());

            var handlers = (IEnumerable<object>)_provider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType)); // Change GetService to GetServices and cast to IEnumerable<object>

            foreach (var handler in handlers)
            {
                await (Task)handlerType
                    .GetMethod("Handle")
                    .Invoke(handler, new object[] { notification, cancellationToken })!;
            }
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var handler = _provider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {request.GetType().Name} not found.");
            }

            return await (Task<TResponse>)handlerType
                .GetMethod("Handle")
                .Invoke(handler, new object[] { request, cancellationToken });
        }
    }
}
