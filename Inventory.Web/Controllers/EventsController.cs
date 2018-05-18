using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventStore eventStore;

        public EventsController(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }
        // GET: Events
        public async Task<ActionResult> Index(Guid id)
        {
            var events = await eventStore.GetEventsAsync(id, 0);

            return View(events);
        }
    }
}