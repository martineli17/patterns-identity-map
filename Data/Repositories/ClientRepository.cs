using Data.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IIdentityMapQueryCommand _identityMap;

        public ClientRepository(DatabaseContext databaseContext, IIdentityMapQueryCommand identityMap)
        {
            _databaseContext = databaseContext;
            _identityMap = identityMap;
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken)
        {
           await _databaseContext.Client.AddAsync(client, cancellationToken);
        }

        public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken)
        {
            var key = $"{nameof(Client)}_{nameof(GetByNameAsync)}_all";
            return await _identityMap.ExecuteAsync(key, () => _databaseContext.Client.ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var key = $"{nameof(Client)}_{nameof(GetByNameAsync)}_{name}";
            return await _identityMap.ExecuteAsync(key, () => _databaseContext.Client.Where(x => x.Name.ToLower().Contains(name)).ToListAsync(cancellationToken));
        }
    }
}
