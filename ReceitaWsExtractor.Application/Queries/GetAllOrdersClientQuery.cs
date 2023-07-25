using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Queries;

public class GetAllOrdersClientQuery : IRequest<ICollection<Order>>
{
    public Guid Id { get; set; }

    public class GetAllOrdersClientQueryHandler : IRequestHandler<GetAllOrdersClientQuery, ICollection<Order>>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;

        public GetAllOrdersClientQueryHandler(IClientRepository repository, IMediator mediator)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ICollection<Order>> Handle(GetAllOrdersClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _repository.FindById(request.Id);

            if (client == null)
            {
                throw new ArgumentException("The client doesn't exist");
            }

            if (client.Orders.Count == 0)
            {
                throw new ArgumentException("The client doesn't have any Orders");
            }

            var orders = client.Orders;

            return orders;
        }
    }
}
