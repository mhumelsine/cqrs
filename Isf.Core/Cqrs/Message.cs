using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class Message
    {
        public readonly Guid Id;

        public Message()
        {
            Id = Guid.NewGuid();
        }
    }

    public abstract class Query : Message { }

    public abstract class Command : Message { }

    public abstract class CommandWithAggregateRootId : Command
    {
        public Guid AggregateRootId { get; set; }
    }   
}
