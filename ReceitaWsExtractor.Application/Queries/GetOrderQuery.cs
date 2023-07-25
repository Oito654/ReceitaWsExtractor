using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Queries;

public class GetOrderQuery : IRequest<Order>
{
    public Guid ClientId { get; set; }
    public Guid OrderId { get; set; }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;

        public GetOrderQueryHandler(IClientRepository repository, IMediator mediator)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var client = await _repository.FindById(request.ClientId);

            if(client == null)
            {
                throw new ArgumentException("The client doesn't exist");
            }

            if(client.Orders.Count== 0)
            {
                throw new ArgumentException("The client doesn't have any Orders");
            }

            var order = client.Orders.Where(o => o.Id==request.OrderId).FirstOrDefault();

            if(order == null)
            {
                throw new ArgumentException("The Order doesn't exist");
            }

            return order;
        }
    }
}
