using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Commands;

public class AddTokenCommand : IRequest<Client>
{
    public string Token { get; set; }
    public Client Client { get; set; }

    public class AddTokenCommandHandler : IRequestHandler<AddTokenCommand, Client>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;

        public AddTokenCommandHandler(IClientRepository repository, IMediator mediator)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Client> Handle(AddTokenCommand command, CancellationToken cancellationToken)
        {
            await _repository.AddToken(command.Token, command.Client);

            return command.Client;
        }
    }
}
