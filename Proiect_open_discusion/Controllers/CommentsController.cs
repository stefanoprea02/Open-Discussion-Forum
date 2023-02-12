using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_open_discusion.Data;
using Proiect_open_discusion.Models;
using System.Data;

namespace Proiect_open_discusion.Controllers
{
    [Authorize(Roles = "Administrator,Moderator,User")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public CommentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public ActionResult New(int id)
        {
            var subject = db.Subjects.Find(id);
            ViewBag.Subject = subject;
            return View();
        }

        [HttpPost]
        public ActionResult New([FromForm] Comment comment)
        {
            comment.TimePosted = DateTime.Now;
            comment.UserId = userManager.GetUserId(User);
            comment.UserName = userManager.GetUserName(User);
            comment.ReplyFor = "";

            var aux_subject = db.Subjects.Find(comment.SubjectId);
            comment.Subject = aux_subject;

            Console.WriteLine(string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Subjects/Show?id=" + comment.SubjectId);
            }
            else
            {
                ViewBag.UserId = userManager.GetUserId(User);
                var subject = db.Subjects.Find(comment.SubjectId);
                ViewBag.Subject = subject;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }

        [HttpPost]
        public ActionResult Edit(int id, Comment updatedComment)
        {
            if (ModelState.IsValid)
            {
                Comment comment = db.Comments.Find(id);
                comment.Text = updatedComment.Text;
                db.SaveChanges();
                return RedirectToAction("Show", "Subjects", new {id = comment.SubjectId});
            }
            else
            {
                ViewBag.Comment = db.Comments.Find(id);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Include("User")
                                    .Where(com => com.Id == id)
                                    .First();

            var subject = db.Subjects.Include("Comments")
                                         .Include("User")
                                         .Include("Category")
                                         .Where(subject => subject.Id == comment.SubjectId)
                                         .First();
            
            if (comment.UserId.ToString() == userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Administator"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            
            ViewBag.Subject = subject;
            ViewBag.UserId = userManager.GetUserId(User);
            return View("../Subjects/Show");
        }
    }
}
