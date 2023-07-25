namespace ReceitaWsExtractor.Domain.Entities;

public class ClientToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public bool IsValid { get; set; }

    public ClientToken(Guid id, string token, bool isValid)
    {
        Id = id;
        Token = token;
        IsValid = isValid;
    }
}
