﻿using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Core.Interface
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken = default);

        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification;
    }
}
