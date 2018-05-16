using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class DomainEvent : Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventTimestamp { get; set; }
        public Guid AggregateRootId { get; set; }
        public string UserCreated { get; set; }
        public int EventSequence { get; set; }

        public string EventData { get; set; }

        public DomainEvent()
        {

        }

        public DomainEvent(Guid aggregateRootId, int sequence, string userCreated)
        {
            EventTimestamp = DateTime.Now;
            AggregateRootId = aggregateRootId;
            UserCreated = userCreated;
            EventSequence = sequence;
            EventName = GetType().Name;
        }
    }

    [Table("DOMAIN_EVENT")]
    public class DomainEventMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventTimestamp { get; set; }
        public Guid AggregateRootId { get; set; }
        public string UserCreated { get; set; }
        public int EventSequence { get; set; }
    }

    [Table("DOMAIN_EVENT_DATA")]
    public class DomainEventData
    {
        [Key, ForeignKey("DomainEventMeta")]
        public Guid EventId { get; set; }
        public string SerializedEvent { get; set; }
        public DomainEventMeta DomainEventMeta { get; set; }
    }
}
