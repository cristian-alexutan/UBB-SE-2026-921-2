using AirportWebApp.Infrastructure;
using AirportWebApp.Models.DutyFree;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class DutyFreeController : Controller
{
    private readonly WebUserSession session;
    private readonly IShopService shopService;
    private readonly IManagerService managerService;

    public DutyFreeController(
        WebUserSession session,
        IShopService shopService,
        IManagerService managerService)
    {
        this.session = session;
        this.shopService = shopService;
        this.managerService = managerService;
    }

    public IActionResult Index(string? search)
    {
        IEnumerable<Shop> shops;
        if (!string.IsNullOrWhiteSpace(search))
        {
            shops = shopService.SearchByName(search);
        }
        else
        {
            shops = shopService.GetAllAvailableShops();
        }

        var model = new ShopListViewModel
        {
            Shops = shops.ToList(),
            SearchQuery = search ?? string.Empty,
            UserRole = session.DutyFreeRole,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddShop(ShopFormModel form)
    {
        if (session.IsDutyFreeManager && ModelState.IsValid)
        {
            var manager = managerService.GetManagerById(session.DutyFreeUserId);
            shopService.AddShop(new Shop(form.Name, form.Type, manager!));
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult EditShopForm(int id)
    {
        if (!session.IsDutyFreeManager)
        {
            return Forbid();
        }

        var shop = shopService.GetAllAvailableShops().FirstOrDefault(s => s.Id == id);
        if (shop == null)
        {
            return NotFound();
        }

        var form = new ShopFormModel
        {
            Id = shop.Id,
            Name = shop.Name,
            Type = shop.Type,
        };

        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditShop(ShopFormModel form)
    {
        if (!session.IsDutyFreeManager)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            var existing = shopService.GetAllAvailableShops().FirstOrDefault(s => s.Id == form.Id);
            if (existing != null)
            {
                existing.Name = form.Name;
                existing.Type = form.Type;
                shopService.UpdateShop(existing);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteShop(int id)
    {
        if (!session.IsDutyFreeManager)
        {
            return Forbid();
        }

        shopService.DeleteShop(id);
        return RedirectToAction(nameof(Index));
    }
}
