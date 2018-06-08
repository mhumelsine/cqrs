using System.Threading.Tasks;
using Inventory.Inventory;
using Isf.Core.Cqrs.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    public class InventoryController : CqrsMvcController
    {
        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            return await DispatchQueryAsync(new GetTopInventoryMastersQuery());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(GetMasterByIdQuery query)
        {
            return await DispatchQueryAsync(query, q =>
            {
                var inventoryMaster = q.Result as InventoryMaster;

                if (inventoryMaster == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(inventoryMaster);
            });
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInventoryMasterCommand command)
        {
            return await DispatchCommandAsync(command, c => RedirectToAction(nameof(Index)));
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(GetMasterByIdQuery query)
        {
            return await DispatchQueryAsync(query, q =>
            {
                var inventoryMaster = q.Result as InventoryMaster;

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
            });            
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateInventoryMasterCommand command)
        {
            return await DispatchCommandAsync(command, c => RedirectToAction(nameof(Index)));
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