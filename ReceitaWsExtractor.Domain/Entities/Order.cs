namespace ReceitaWsExtractor.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string Cnpj { get; set; }
    public string Result { get; set; }
}
