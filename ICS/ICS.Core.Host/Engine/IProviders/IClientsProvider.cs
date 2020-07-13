using ICS.Core.Host.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.IProviders
{
    public interface IClientsProvider
    {
        IQueryable<Client> GetClusters();
        Task InsertAsync(Client client);
        Task UpdateAsync(Client client);
    }
}
