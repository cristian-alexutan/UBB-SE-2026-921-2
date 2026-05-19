using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirportApp.Data;
using AirportApp.Data.Domain;

using EmployeeFlight = AirportApp.Data.Domain.EmployeeFlight;

namespace AirportAPI.Controllers.New_Controllers
{
    public class EmployeeFlightsController : Controller
    {
        private readonly AppDbContext context;

        public EmployeeFlightsController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: EmployeeFlights
        public async Task<IActionResult> Index()
        {
            return View(await context.EmployeeFlights.ToListAsync());
        }

        // GET: EmployeeFlights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFlight = await context.EmployeeFlights
                .FirstOrDefaultAsync(m => m.Employee.Id == id);
            if (employeeFlight == null)
            {
                return NotFound();
            }

            return View(employeeFlight);
        }

        // GET: EmployeeFlights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeFlights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("")] EmployeeFlight employeeFlight)
        {
            if (ModelState.IsValid)
            {
                context.Add(employeeFlight);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeFlight);
        }

        // GET: EmployeeFlights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFlight = await context.EmployeeFlights.FindAsync(id);
            if (employeeFlight == null)
            {
                return NotFound();
            }
            return View(employeeFlight);
        }

        // POST: EmployeeFlights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("")] EmployeeFlight employeeFlight)
        {
            if (id != employeeFlight.Employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(employeeFlight);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeFlightExists(employeeFlight.Employee.Id))
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
            return View(employeeFlight);
        }

        // GET: EmployeeFlights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeFlight = await context.EmployeeFlights
                .FirstOrDefaultAsync(m => m.Employee.Id == id);
            if (employeeFlight == null)
            {
                return NotFound();
            }

            return View(employeeFlight);
        }

        // POST: EmployeeFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeFlight = await context.EmployeeFlights.FindAsync(id);
            if (employeeFlight != null)
            {
                context.EmployeeFlights.Remove(employeeFlight);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeFlightExists(int id)
        {
            return context.EmployeeFlights.Any(e => e.Employee.Id == id);
        }
    }
}
