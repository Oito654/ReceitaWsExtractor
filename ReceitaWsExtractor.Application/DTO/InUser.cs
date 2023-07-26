using Microsoft.AspNetCore.Http;

namespace ReceitaWsExtractor.Application.DTO;

public class InUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
