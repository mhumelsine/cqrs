using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Inventory;
using Inventory.Web.Common;
using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;

        public InventoryController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        // GET: Inventory
        public async Task<ActionResult> Index()
        {
            var query = new GetTopInventoryMastersQuery();

            var result = await queryBus.PublishAsync(query);

            return View(result.Result);
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(GetMasterByIdQuery query)
        {
            var result = await queryBus.PublishAsync(query);
            var inventoryMaster = result.Result as InventoryMaster;

            if (inventoryMaster == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(inventoryMaster);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateInventoryMasterCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(command);
                }

                var result = await commandBus.PublishAsync(command);

                if (result.State == ExecutionStatus.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelErrors(result.Notification);
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Inventory/Edit/5
        public async Task<ActionResult> Edit(GetMasterByIdQuery query)
        {
            var result = await queryBus.PublishAsync(query);
            var inventoryMaster = result.Result as InventoryMaster;

            if (inventoryMaster == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //maybe need auto mapper here?
            var command = new UpdateInventoryMasterCommand
            {
                LIN = inventoryMaster.LIN,
                AggregateRootId = inventoryMaster.AggregateRootId,
                GeneralNomenclature = inventoryMaster.GeneralNomenclature,
                IsGArmy = inventoryMaster.IsGArmy,
                Status = inventoryMaster.Status,
                TrackingType = inventoryMaster.TrackingType
            };

            return View(command);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateInventoryMasterCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(command);
                }

                var result = await commandBus.PublishAsync(command);

                if (result.State == ExecutionStatus.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelErrors(result.Notification);
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}