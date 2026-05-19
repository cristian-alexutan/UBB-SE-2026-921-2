using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirportApp.Data;
using AirportApp.Data.Domain;

using Runway = AirportApp.Data.Domain.Runway;

namespace AirportAPI.Controllers.New_Controllers
{
    public class RunwaysController : Controller
    {
        private readonly AppDbContext context;

        public RunwaysController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: Runways
        public async Task<IActionResult> Index()
        {
            return View(await context.Runways.ToListAsync());
        }

        // GET: Runways/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runway = await context.Runways
                .FirstOrDefaultAsync(m => m.Id == id);
            if (runway == null)
            {
                return NotFound();
            }

            return View(runway);
        }

        // GET: Runways/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Runways/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,HandleTime")] Runway runway)
        {
            if (ModelState.IsValid)
            {
                context.Add(runway);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(runway);
        }

        // GET: Runways/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runway = await context.Runways.FindAsync(id);
            if (runway == null)
            {
                return NotFound();
            }
            return View(runway);
        }

        // POST: Runways/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HandleTime")] Runway runway)
        {
            if (id != runway.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(runway);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunwayExists(runway.Id))
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
            return View(runway);
        }

        // GET: Runways/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runway = await context.Runways
                .FirstOrDefaultAsync(m => m.Id == id);
            if (runway == null)
            {
                return NotFound();
            }

            return View(runway);
        }

        // POST: Runways/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runway = await context.Runways.FindAsync(id);
            if (runway != null)
            {
                context.Runways.Remove(runway);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunwayExists(int id)
        {
            return context.Runways.Any(e => e.Id == id);
        }
    }
}
