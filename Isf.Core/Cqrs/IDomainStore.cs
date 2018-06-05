using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IDomainStore
    {
        Task<TAggregateRoot> GetByIdAsync<TAggregateRoot>(Guid aggregateRootId) where TAggregateRoot : AggregateRoot, new();
        Task<TAggregateRoot> GetExistingByIdAsync<TAggregateRoot>(Guid aggregateRootId) where TAggregateRoot : AggregateRoot, new();
        Task DeleteAsync<TAggregateRoot>(Guid aggregateRootId) where TAggregateRoot : AggregateRoot, new();
        Task SaveAsync<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot, new();

    }
}
