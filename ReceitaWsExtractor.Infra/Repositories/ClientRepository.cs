using Microsoft.EntityFrameworkCore;
using ReceitaWsExtractor.Domain.Entities;
using ReceitaWsExtractor.Domain.Interfaces;
using ReceitaWsExtractor.Infra.Context;

namespace ReceitaWsExtractor.Infra.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ReceitaWsExtractorDbContext _context;

    public ClientRepository(ReceitaWsExtractorDbContext context)
    {
        _context = context;
    }

    #region Client
    public async Task<Client> FindById(Guid id)
    {
        return _context.Client.Where(c => c.Id == id).FirstOrDefault();
    }
    public async Task AddToken(string token, Client client)
    {
        var bearerToken = new ClientToken(client.Id, token, true);

        client.Token = bearerToken;
        await _context.SaveChangesAsync();
    }
    public async Task InvalidToken(Client client)
    {
        client.Token.IsValid = false;
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Orders
    public async Task AddOrder(Order order, Guid clientId)
    {
        var client = await _context.Client.FindAsync(clientId);
        client.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    public async Task<Order> GetOrder(Guid id, Guid clientId)
    {
        var client = await _context.Client.FindAsync(clientId);
        var tag = client.Orders.Where(x => x.Id == id).FirstOrDefault();

        return tag;
    }
    public async Task<ICollection<Order>> GetAllOrdersClient(Guid clientId)
    {
        var client = await _context.Client.FindAsync(clientId);
        return client.Orders;
    }
    #endregion
}
