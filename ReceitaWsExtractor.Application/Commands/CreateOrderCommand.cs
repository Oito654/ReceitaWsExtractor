using MediatR;
using ReceitaWsExtractor.Application.HttpClient;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public string Cnpj { get; set; }
    public string ClientId { get; set; }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;
        private readonly IConsultaCnpjHttpClient _consultaCnpj;

        public CreateOrderCommandHandler(IClientRepository repository, IMediator mediator, IConsultaCnpjHttpClient consultaCnpj
            )
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository)); ;

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));

            _consultaCnpj = consultaCnpj
                ?? throw new ArgumentNullException(nameof(consultaCnpj));
        }

        public async Task<Order> Handle(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken)
        {
            var cnpj = createOrderCommand.Cnpj;

            var result = await _consultaCnpj.GetCnpjDataAsync(cnpj);

            var order = new Order(Guid.NewGuid(), cnpj, result);

            await _repository.AddOrder(order, Guid.Parse(createOrderCommand.ClientId));

            return order;
        }
    }
}
