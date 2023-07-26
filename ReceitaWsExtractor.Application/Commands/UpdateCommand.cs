using MediatR;
using Microsoft.AspNetCore.Http;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;

namespace ReceitaWsExtractor.Application.Commands;

public class UpdateCommand : IRequest<Client>
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;

    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Client>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMediator _mediator;

        public UpdateCommandHandler(IClientRepository clientRepository, IMediator mediator)
        {
            _clientRepository = clientRepository
                ?? throw new ArgumentNullException(nameof(clientRepository));

            _mediator = mediator
                ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Client> Handle(UpdateCommand command, CancellationToken cancellationToken)
        {

            var client = await _clientRepository.FindById(Guid.Parse(command.Id));

            if (client == null)
            {
                throw new ArgumentException("The client doesn't exist");
            }

            
            client.Name = command.Name;
            client.Surname = command.Surname;
            client.Email = command.Email;
            client.UpdatedIn = DateTime.Now;

            await _clientRepository.Update(client);

            return client;
        }
    }
}
