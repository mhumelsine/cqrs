using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Inventory
{
    public class InventoryMasterCreatedEvent : DomainEvent
    {
        public readonly string LIN;
        public readonly string GeneralNomenclature;
        public readonly bool IsGArmy;
        public readonly int TrackingType;

        public InventoryMasterCreatedEvent(
            Guid aggregateRootId,
            string LIN,
            string generalNomenclature,
            bool isGArmy,
            int trackingType,
            string userCreated)
            : base(aggregateRootId, 1, userCreated)
        {
            this.AggregateRootId = aggregateRootId;
            this.LIN = LIN;
            this.GeneralNomenclature = generalNomenclature;
            this.IsGArmy = isGArmy;
            this.TrackingType = trackingType;
        }
    }

    public class InventoryMasterDeletedEvent : DomainEvent { }

    public class InventoryMasterUpdatedEvent : DomainEvent
    {
        public readonly string LIN;
        public readonly string GeneralNomenclature;
        public readonly bool IsGArmy;
        public readonly int TrackingType;

        public InventoryMasterUpdatedEvent(
            Guid itemId,
            string LIN,
            string generalNomenclature,
            bool isGArmy,
            int trackingType,
            string userModified)
            : base(itemId, 0, userModified)
        {
            this.AggregateRootId = itemId;
            this.LIN = LIN;
            this.GeneralNomenclature = generalNomenclature;
            this.IsGArmy = isGArmy;
            this.TrackingType = trackingType;
        }
    }

    public class LinChangedEvent : DomainEvent
    {
        public readonly string LIN;

        public LinChangedEvent(
            Guid itemId,
            string lin,
            string userChanged
            )
            :base(itemId, 0, userChanged)
        {
            LIN = lin;
        }
    }
}
