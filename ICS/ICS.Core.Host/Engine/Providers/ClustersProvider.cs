using ICS.Core.Dtos.Outcome;
using ICS.Core.Host.Contracts;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using ICS.Core.Host.Engine.IProviders;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.Providers
{
    public class ClustersProvider : IClustersProvider
    {
        private PostgresContext _context;
        public ClustersProvider(PostgresContext context)
        {
            _context = context;
        }
        public IQueryable<Cluster> GetClusters()
        {
            return _context.Clusters.AsQueryable();
        }

        public List<OutcomeCluster> GetOutcomeClusters()
        {
            var clusters = _context.Clusters.Include(x => x.KeepAlives).AsQueryable();
            var outcomeClusters = clusters.Select(x => new OutcomeCluster()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                IPAddress = x.IPAddress.ToString(),
                IsConfirmed = x.IsConfirmed,
                Name = x.Name,
                Port = x.Port,
                Uuid = x.Uuid.ToString(),
                LastKeepAlive = x.KeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault().CreatedAt
            }).ToList();
            return outcomeClusters;
        }

        public StatusAutorization GetStatusAutorization(Guid token)
        {
            var cluster = _context.Clusters.Include(x => x.Token).FirstOrDefault(x => x.Token.Value == token);
            var statusAutorization = new StatusAutorization();
            if (cluster == null) statusAutorization.StatusCode = (int)HttpStatusCode.BadRequest;
            else if (DateTime.UtcNow - cluster.Token.ExpiredAt > new TimeSpan(0, 0, 0)) statusAutorization.StatusCode = (int)HttpStatusCode.Forbidden;
            else if (!cluster.IsConfirmed) statusAutorization.StatusCode = (int)HttpStatusCode.Forbidden;
            else
            {
                statusAutorization.StatusCode = (int)HttpStatusCode.OK;
                statusAutorization.Cluster = cluster;
            }
            return statusAutorization;
        }

        public async Task InsertAsync(Cluster cluster)
        {
            await _context.Clusters.AddAsync(cluster);
            await _context.SaveChangesAsync();
        }
        public async Task InsertKeepAliveAsync(ClusterKeepAlive keepAlive)
        {
            await _context.ClusterKeepAlives.AddAsync(keepAlive);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cluster cluster)
        {
            _context.Clusters.Update(cluster);
            await _context.SaveChangesAsync();
        }
    }

}
