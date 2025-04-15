using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Core.Interface
{
    public interface INotificationHandler<TNotification>
        where TNotification : INotification
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
