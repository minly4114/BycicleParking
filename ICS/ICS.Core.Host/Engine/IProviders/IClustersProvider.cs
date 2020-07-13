using ICS.Core.Dtos.Outcome;
using ICS.Core.Host.Contracts;
using ICS.Core.Host.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.IProviders
{
    public interface IClustersProvider
    {
        IQueryable<Cluster> GetClusters();
        List<OutcomeCluster> GetOutcomeClusters();
        Task InsertAsync(Cluster cluster);
        Task UpdateAsync(Cluster cluster);
        StatusAutorization GetStatusAutorization(Guid token);
        Task InsertKeepAliveAsync(ClusterKeepAlive keepAlive);
    }
}
