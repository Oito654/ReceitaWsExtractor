using MediatR;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Commands;

public class InvalidTokenCommand : IRequest<Client>
{
    public Client Client { get; set; }

    public class InvalidTokenCommandHandler : IRequestHandler<InvalidTokenCommand, Client>
    {
        private readonly IClientRepository _repository;
        private readonly IMediator _mediator;

        public InvalidTokenCommandHandler(IClientRepository repository, IMediator mediator)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Client> Handle(InvalidTokenCommand command, CancellationToken cancellationToken)
        {
            await _repository.InvalidToken(command.Client);

            return command.Client;
        }
    }
}
