using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isf.Core.Cqrs;

namespace Inventory.Web.Controllers
{
    public class DomainEventsController : Controller
    {
        private readonly EventDbContext _context;

        public DomainEventsController(EventDbContext context)
        {
            _context = context;
        }

        // GET: DomainEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.DomainEvents.ToListAsync());
        }

        // GET: DomainEvents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domainEvent = await _context.DomainEvents
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (domainEvent == null)
            {
                return NotFound();
            }

            return View(domainEvent);
        }

        // GET: DomainEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DomainEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventTimestamp,AggregateRootId,UserCreated,EventSequence,EventData")] DomainEvent domainEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(domainEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domainEvent);
        }

        // GET: DomainEvents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domainEvent = await _context.DomainEvents.SingleOrDefaultAsync(m => m.EventId == id);
            if (domainEvent == null)
            {
                return NotFound();
            }
            return View(domainEvent);
        }

        // POST: DomainEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EventId,EventName,EventTimestamp,AggregateRootId,UserCreated,EventSequence,EventData")] DomainEvent domainEvent)
        {
            if (id != domainEvent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domainEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomainEventExists(domainEvent.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(domainEvent);
        }

        // GET: DomainEvents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domainEvent = await _context.DomainEvents
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (domainEvent == null)
            {
                return NotFound();
            }

            return View(domainEvent);
        }

        // POST: DomainEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var domainEvent = await _context.DomainEvents.SingleOrDefaultAsync(m => m.EventId == id);
            _context.DomainEvents.Remove(domainEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DomainEventExists(long id)
        {
            return _context.DomainEvents.Any(e => e.EventId == id);
        }
    }
}
