using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using AirportApp.Data;
using AirportApp.Data.Domain;

using ShopItem = AirportApp.Data.Domain.ShopItem;

namespace AirportAppWebAPI.Controllers
{
    public class ShopItemsController : Controller
    {
        private readonly AppDbContext context;

        public ShopItemsController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: ShopItems
        public async Task<IActionResult> Index()
        {
            return View(await context.ShopItems.ToListAsync());
        }

        // GET: ShopItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await context.ShopItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // GET: ShopItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,Price,Photo,Name,Description")] ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                context.Add(shopItem);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopItem);
        }

        // GET: ShopItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await context.ShopItems.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }
            return View(shopItem);
        }

        // POST: ShopItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,Price,Photo,Name,Description")] ShopItem shopItem)
        {
            if (id != shopItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(shopItem);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopItemExists(shopItem.Id))
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
            return View(shopItem);
        }

        // GET: ShopItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopItem = await context.ShopItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // POST: ShopItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopItem = await context.ShopItems.FindAsync(id);
            if (shopItem != null)
            {
                context.ShopItems.Remove(shopItem);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopItemExists(int id)
        {
            return context.ShopItems.Any(e => e.Id == id);
        }
    }
}
