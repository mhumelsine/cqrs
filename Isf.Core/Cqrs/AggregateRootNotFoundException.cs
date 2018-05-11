using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class AggregateRootNotFoundException<TAggregateRoot> : Exception
    {
        public AggregateRootNotFoundException(params object[] keys)
            :base($"AggregateRoot '{typeof(TAggregateRoot)}' with key '{string.Join(",", keys)}' not found")
        {

        }
    }
}
