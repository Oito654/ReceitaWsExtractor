using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Queries;

public class GetAllClientsQuery : IRequest<ICollection<Client>?>
{
    public class GetAllClientesQueryHandler : IRequestHandler<GetAllClientsQuery, ICollection<Client>?>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMediator _mediator;

        public GetAllClientesQueryHandler(IClientRepository clientRepository, IMediator mediator)
        {
            _clientRepository = clientRepository
                ?? throw new ArgumentNullException(nameof(clientRepository));

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ICollection<Client>?> Handle(GetAllClientsQuery getAllClientsQuery, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetAll();

            if (client == null)
            {
                return default;
            }

            return client;
        }
    }
}