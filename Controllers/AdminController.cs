using EquipmentRental.Models;
using EquipmentRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EquipmentRental.ViewModels;

namespace EquipmentRental.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEquipmentService _equipmentService;
        private readonly IRentalRequestService _requestService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser> _userManager;

        public AdminController(IUserService userService, IEquipmentService equipmentService, IRentalRequestService requestService,
            Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser> userManager)
        {
            _userService = userService;
            _equipmentService = equipmentService;
            _requestService = requestService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            var equipment = await _equipmentService.GetAllAsync();
            var requests = await _requestService.GetAllAsync();

            var model = new AdminDashboardViewModel
            {
                UserCount = users.Count(),
                EquipmentCount = equipment.Count(),
                RequestCount = requests.Count(),
                AwaitingCount = requests.Count(r => r.Status == "awaiting")
            };

            return View(model);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(Models.ApplicationUser user)
        {
            if (!ModelState.IsValid) return View(user);
            await _userService.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Users));
        }
    }
}