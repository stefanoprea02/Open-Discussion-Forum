using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_open_discusion.Data;
using Proiect_open_discusion.Models;

namespace Proiect_open_discusion.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, User")]
    public class CategoriesController : Controller
    {
        public readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var categories = db.Categories.Include("Subjects").ToList();
            ViewBag.Categories = categories;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult New(Category category)
        {
            if(ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }else
            {
                return View(category);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, Category updatedCategory)
        {
            if(ModelState.IsValid)
            {
                Category category = db.Categories.Find(id);
                category.Name = updatedCategory.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }else
            {
                ViewBag.Category = db.Categories.Find(id);
                return View();
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Include("Subjects").Where(category => category.Id == id).First();
            foreach (var subject in category.Subjects.ToList())
            {
                var subject2 = db.Subjects.Include("Comments").Where(aux => aux.Id == subject.Id).First();
                foreach (var comment in subject2.Comments.ToList())
                {
                    db.Comments.Remove(comment);
                }
                db.SaveChanges();

                db.Subjects.Remove(subject2);
                db.SaveChanges();
            }
            db.SaveChanges();

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
