using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Commands;
using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ICommandBus commandBus;

        public InventoryController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateInventoryItemCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(command);
                }

                var result = await commandBus.PublishAsync(command);

                if(result.State == ExecutionStatus.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //put in lib function
                    foreach(var prop in result.Notification.ErrorDictionary)
                    {
                        foreach(var error in prop.Value)
                        {
                            ModelState.AddModelError(prop.Key, error);
                        }
                        
                    }
                    return View();
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
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