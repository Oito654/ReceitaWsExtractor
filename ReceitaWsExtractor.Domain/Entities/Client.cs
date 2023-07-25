using Microsoft.AspNetCore.Identity;

namespace ReceitaWsExtractor.Domain.Entities;

public class Client : IdentityUser<Guid>
{
    public Client() { }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public override string Email { get; set; } = string.Empty;
    public DateTime CreatedIn { get; set; }
    public DateTime? UpdatedIn { get; set; }
}
