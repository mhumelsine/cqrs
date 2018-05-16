using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Inventory
{
    public class InventoryMaster : AggregateRoot
    {        
        public int Id { get; set; }
        public string LIN { get; set; }

        public string GeneralNomenclature { get; set; }

        public bool IsGArmy { get; set; }

        public int TrackingType { get; set; }

        public int Status { get; set; }

        public InventoryMaster()
        {

        }

        public InventoryMaster(
            string LIN,
            string generalNomenclature,
            bool isGArmy,
            int trackingType,
            string userCreated)
        {
            Apply(new InventoryMasterCreatedEvent(
                Guid.NewGuid(),
                LIN,
                generalNomenclature,
                isGArmy,
                trackingType,
                userCreated));
        }

        public void Edit(
            string LIN,
            string generalNomenclature,
            bool isGArmy,
            int trackingType,
            string userModified)
        {
            Apply(new InventoryMasterUpdatedEvent(
                AggregateRootId,
                LIN,
                generalNomenclature,
                isGArmy,
                trackingType,
                userModified));
        }

        public void Delete()
        {
        //    Apply(new InventoryMasterDeletedEvent)
        }



        protected void OnInventoryMasterCreated(InventoryMasterCreatedEvent e)
        {
            AggregateRootId = e.AggregateRootId;
            LIN = e.LIN;
            GeneralNomenclature = e.GeneralNomenclature;
            IsGArmy = e.IsGArmy;
            TrackingType = e.TrackingType;
        }

        protected void OnInventoryMasterUpdated(InventoryMasterUpdatedEvent e)
        {
            LIN = e.LIN;
            GeneralNomenclature = e.GeneralNomenclature;
            IsGArmy = e.IsGArmy;
            TrackingType = e.TrackingType;
        }
    }
}
