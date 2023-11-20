using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken cancellationToken);
        Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Client>> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
