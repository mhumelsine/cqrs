using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IHandleQuery<TQuery>
    {
        Task<QueryResult> HandleAsync(TQuery query);
    }
}
