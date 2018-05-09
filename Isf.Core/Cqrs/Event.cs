using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class Event
    {
        public Guid Id { get; set; }

        public Event()
        {
            Id = Guid.NewGuid();
        }
    }
}
