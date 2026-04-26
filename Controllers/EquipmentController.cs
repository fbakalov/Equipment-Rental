using EquipmentRental.Models;
using EquipmentRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentRental.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _service;

        public EquipmentController(IEquipmentService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? q)
        {
            var items = string.IsNullOrWhiteSpace(q) ? await _service.GetAllAsync() : await _service.SearchByNameAsync(q);
            return View(items);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create() => View(new EquipmentItem());

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(EquipmentItem item)
        {
            if (!ModelState.IsValid) return View(item);
            await _service.AddAsync(item);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(EquipmentItem item)
        {
            if (!ModelState.IsValid) return View(item);
            await _service.UpdateAsync(item);
            return RedirectToAction(nameof(Index)); // Updated redirect
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}