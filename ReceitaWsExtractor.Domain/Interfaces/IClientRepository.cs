using ReceitaWsExtractor.Domain.Entities;

namespace ReceitaWsExtractor.Domain.Interfaces;

public interface IClientRepository
{
    #region Client
        Task<ICollection<Client>?> GetAll();
        Task<Client> FindById(Guid id);
        Task AddToken(string token, Client client);
        Task InvalidToken(Client client);
        Task Update(Client client);
    #endregion

    #region Order
    Task AddOrder(Order order, Guid clientId);
        Task<Order> GetOrder(Guid orderId, Guid clientId);
        Task<ICollection<Order>> GetAllOrdersClient(Guid clientId);
    #endregion
}