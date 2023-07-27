using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReceitaWsExtractor.Application.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public string Cnpj { get; set; }
    public string ClientId { get; set; }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;

        static HttpClient client = new HttpClient();

        public CreateOrderCommandHandler(IClientRepository repository, IMediator mediator            )
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository)); ;

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));

        }

        public async Task<Order> Handle(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken)
        {
            var cnpj = createOrderCommand.Cnpj;

            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri($"https://receitaws.com.br/v1/cnpj");
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            var path = $"{client.BaseAddress}/{createOrderCommand.Cnpj}";
            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }
            
            var result = await response.Content.ReadAsStringAsync();

            var order = new Order(Guid.NewGuid(), cnpj, result);

            await _repository.AddOrder(order, Guid.Parse(createOrderCommand.ClientId));

            return order;
        }
    }
}
