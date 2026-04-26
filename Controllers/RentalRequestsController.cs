using EquipmentRental.Models;
using EquipmentRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentRental.Controllers
{
    [Authorize]
    public class RentalRequestsController : Controller
    {
        private readonly IRentalRequestService _service;
        private readonly IEquipmentService _equipmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalRequestsController(IRentalRequestService service, IEquipmentService equipmentService, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _equipmentService = equipmentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var list = await _service.GetByUserIdAsync(user.Id);
            return View(list);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> All()
        {
            var list = await _service.GetAllAsync();
            return View(list);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            await _service.UpdateStatusAsync(id, status);
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Create()
        {
            var items = await _equipmentService.GetAllAsync();

            var model = new EquipmentRental.ViewModels.RentalRequestCreateViewModel
            {
                Items = items,
                Request = new EquipmentRental.Models.RentalRequest { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1) }
            };

            // support quick create from equipment card with itemId
            if (int.TryParse(Request.Query["itemId"], out var itemId))
            {
                var single = await _equipmentService.GetByIdAsync(itemId);
                if (single != null)
                {
                    model.Items = new List<EquipmentRental.Models.EquipmentItem> { single };
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EquipmentRental.ViewModels.RentalRequestCreateViewModel model, [FromForm] Dictionary<int,int> quantities)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (model == null) model = new EquipmentRental.ViewModels.RentalRequestCreateViewModel();

            // bind quantities into the nested request
            if (quantities != null)
            {
                foreach (var q in quantities)
                {
                    if (q.Value <= 0) continue;
                    model.Request.RentalRequestItems.Add(new RentalRequestItem
                    {
                        EquipmentItemId = q.Key,
                        Quantity = q.Value
                    });
                }
            }

            model.Request.UserId = user.Id;

            // Clear model state produced by model binding so we validate the server-populated model.Request
            ModelState.Clear();

            if (!TryValidateModel(model.Request))
            {
                // reload items for view and show validation errors
                model.Items = await _equipmentService.GetAllAsync();
                return View(model);
            }

            try
            {
                var created = await _service.CreateAsync(model.Request);
                return RedirectToAction(nameof(MyRequests));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                // reload equipment list and show create view with error
                model.Items = await _equipmentService.GetAllAsync();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var rr = await _service.GetByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (rr == null) return NotFound();
            if (rr.UserId != user?.Id) return Forbid();
            if (rr.Status != "awaiting") return BadRequest("Only awaiting requests can be deleted");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(MyRequests));
        }
    }
}