using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Core.Interface
{
    public interface IRequestHandler<TRequet, TResponse>
        where TRequet : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequet request, CancellationToken cancellationToken);
    }
}
