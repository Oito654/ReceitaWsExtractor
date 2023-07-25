using ReceitaWsExtractor.Domain.Entities;

namespace ReceitaWsExtractor.Domain.Interfaces;

public interface IClientRepository
{
    #region Client
        Task<ICollection<Client>?> GetAll();
        Task<Client> FindById(Guid id);
    #endregion
}
