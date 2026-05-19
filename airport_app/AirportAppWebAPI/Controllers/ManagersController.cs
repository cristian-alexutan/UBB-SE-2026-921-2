using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using AirportApp.Data;
using AirportApp.Data.Domain;

using Manager = AirportApp.Data.Domain.Manager;

namespace AirportAppWebAPI.Controllers
{
    public class ManagersController : Controller
    {
        private readonly AppDbContext context;

        public ManagersController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            return View(await context.Managers.ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                context.Add(manager);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone")] Manager manager)
        {
            if (id != manager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(manager);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
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
            return View(manager);
        }

        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await context.Managers.FindAsync(id);
            if (manager != null)
            {
                context.Managers.Remove(manager);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return context.Managers.Any(e => e.Id == id);
        }
    }
}
