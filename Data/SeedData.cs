using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.Data
{
    public class SeedData
    {
        private readonly UserManager<Models.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public SeedData(UserManager<Models.ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task SeedAsync()
        {
            var adminRole = "Administrator";
            var userRole = "User";

            if (!await _roleManager.RoleExistsAsync(adminRole))
                await _roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!await _roleManager.RoleExistsAsync(userRole))
                await _roleManager.CreateAsync(new IdentityRole(userRole));

            var adminEmail = "admin@example.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new Models.ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(adminUser, "Admin123!");
                await _userManager.AddToRoleAsync(adminUser, adminRole);
            }

            if (!await _db.EquipmentItems.AnyAsync())
            {
                var items = new List<Models.EquipmentItem>
                {
                    new Models.EquipmentItem { Name = "Projector X100", Description = "1080p portable projector.", AvailableQuantity = 30, ImageUrl = "https://www.viewsonic.com/vsAssetFile/bd/img/slides/0projector/X100-4K/X100-4K_LF02.jpg", Condition = "new" },
                    new Models.EquipmentItem { Name = "Microphone Pro", Description = "Wireless handheld microphone.", AvailableQuantity = 20, ImageUrl = "https://magazin.photosynthesis.bg/210219-large_default/mikrofon-rode-wireless-pro.jpg", Condition = "used" },
                    new Models.EquipmentItem { Name = "Laptop Dell", Description = "15\" laptop for presentations.", AvailableQuantity = 35, ImageUrl = "https://www.psp-bg.com/storage/images/products/3/391_2.jpg", Condition = "new" }                
                };

                _db.EquipmentItems.AddRange(items);
                await _db.SaveChangesAsync();
            }
        }
    }
}
