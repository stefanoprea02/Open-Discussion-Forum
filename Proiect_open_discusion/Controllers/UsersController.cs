using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_open_discusion.Data;
using Proiect_open_discusion.Models;
using System.Data;

namespace Proiect_open_discusion.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, User")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = db.Users.ToList();
            var userRoles = new List<IdentityUserRole<string>>();
            foreach(var user in users){
                userRoles.Add(db.UserRoles.Where(x => x.UserId == user.Id).First());
            }
            var roles = new List<IdentityRole<string>>();
            foreach(var userRole in userRoles)
            {
                roles.Add(db.Roles.Find(userRole.RoleId));
            }
            ViewBag.Roles = roles;
            ViewBag.Users = users;
            return View();
        }

        public IActionResult Show(string id, string type = "Subiecte")
        {
            var user = db.Users.Include("Subjects").Include("Comments").Where(x => x.UserName.Equals(id)).First();
            var userRole = db.UserRoles.Where(x => x.UserId == user.Id).First();
            var role = db.Roles.Find(userRole.RoleId);
            ViewBag.UserId = userManager.GetUserId(User);
            ViewBag.Role = role;
            if(type == "Subiecte")
            {
                ViewBag.Type = "Subiecte";
            }
            else if(type == "Comentarii")
            {
                ViewBag.Type = "Comentarii";
            }
            return View(user);
        }

        public IActionResult GiveMod(string id)
        {
            var user = db.Users.Where(x => x.UserName.Equals(id)).First();
            var userRole = db.UserRoles.Where(x => x.UserId == user.Id).First();
            var role = db.Roles.Where(x => x.Name.Equals("Moderator")).First();

            db.UserRoles.Remove(userRole);

            db.UserRoles.Add(
                    new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    }
            );

            db.SaveChanges();

            return RedirectToAction("Show", new { id = user.UserName });
        }

        public IActionResult TakeMod(string id)
        {
            var user = db.Users.Where(x => x.UserName.Equals(id)).First();
            var userRole = db.UserRoles.Where(x => x.UserId == user.Id).First();
            var role = db.Roles.Where(x => x.Name.Equals("User")).First();

            db.UserRoles.Remove(userRole);

            db.UserRoles.Add(
                    new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    }
            );

            db.SaveChanges();

            return RedirectToAction("Show", new { id = user.UserName });
        }
    }
}
