using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using ICS.Core.Host.Engine.IProviders;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.Providers
{
    public class ClientsProvider : IClientsProvider
    {
        private readonly PostgresContext _context;

        public ClientsProvider(PostgresContext context)
        {
            _context = context;
        }

        public IQueryable<Client> GetClusters()
        {
            return _context.Clients.AsQueryable();
        }

        public async Task InsertAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
