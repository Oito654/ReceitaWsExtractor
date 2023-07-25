namespace ReceitaWsExtractor.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string Cnpj { get; set; }
    public string Result { get; set; }

    public Order(Guid id, string cnpj, string result)
    {
        Id = id;
        Cnpj = cnpj;
        Result = result;
    }
}
