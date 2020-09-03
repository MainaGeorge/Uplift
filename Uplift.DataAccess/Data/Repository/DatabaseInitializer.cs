using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Data.Repository
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public DatabaseInitializer(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        public void InitializeDatabase()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("error occurred");
            }

            if (_db.Roles.Any(r => r.Name == AppConstants.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(AppConstants.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(AppConstants.Manager)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Maina George"

            }, _config.GetValue<string>("adminPassword")).GetAwaiter().GetResult();

            var user = _db.ApplicationUsers.SingleOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, AppConstants.Admin).GetAwaiter().GetResult();

        }
    }
}
