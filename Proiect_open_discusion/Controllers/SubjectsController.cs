using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect_open_discusion.Data;
using Proiect_open_discusion.Models;
using System.Data;

namespace Proiect_open_discusion.Controllers
{
    [Authorize(Roles = "Administrator,Moderator,User")]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SubjectsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public ActionResult Index(string SearchText, string sortType, string sortOrder, int id, int pg = 1)
        {
            ViewData["SortOrder"] = sortOrder;
            ViewData["SortType"] = sortType;
            ViewData["SearchText"] = SearchText;

            var category = db.Categories.Find(id);
            ViewBag.Category = category;

            List<Subject> subjs = db.Subjects.Where(subject => subject.CategoryId == id)
                                             .Include("User")
                                             .OrderByDescending(x => x.TimePosted)
                                             .ToList();
            if (SearchText != "" && SearchText != null)
            {
                ViewBag.SearchText = SearchText;
                subjs = (List<Subject>)subjs.Where(n => n.Title.Contains(SearchText) || n.Text.Contains(SearchText)).ToList();
            }

            if(sortOrder == "asc")
            {
                if(sortType == "name")
                {
                    subjs = (List<Subject>)subjs.OrderBy(a => a.Title).ToList();
                }
                else
                {
                    subjs = (List<Subject>)subjs.OrderBy(a => a.TimePosted).ToList();
                }
            }
            else
            {
                if (sortType == "name")
                {
                    subjs = (List<Subject>)subjs.OrderByDescending(a => a.Title).ToList();
                }
                else
                {
                    subjs = (List<Subject>)subjs.OrderByDescending(a => a.TimePosted).ToList();
                }
            }

            const int pageSize = 4;
            if (pg < 1)
                pg = 1;
            int recsCount = subjs.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = subjs.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            ViewBag.Subjects = data;
            return View(data);
        }


        public ActionResult Show(int id)
        {
            var subject = db.Subjects.Include("Comments")
                                     .Include("User")
                                     .Include("Category")
                                     .Where(subject => subject.Id == id)
                                     .First();
            ViewBag.Subject = subject;
            ViewBag.UserId = userManager.GetUserId(User);
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            Subject subject = new Subject();
            subject.Categ = GetAllCategories();
            return View(subject);
        }

        [HttpPost]
        public ActionResult New(Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.TimePosted = DateTime.Now;
                subject.UserId = userManager.GetUserId(User);

                var category = db.Categories.Find(subject.CategoryId);
                subject.Category = category;

                /*
                var user = db.Users.Find(userManager.GetUserId(User));
                subject.User = user;
                */

                db.Subjects.Add(subject);
                db.SaveChanges();
                return Redirect("/Subjects/Show?id=" + subject.Id);
            }
            else
            {
                subject.Categ = GetAllCategories();
                return View(subject);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var subject = db.Subjects.Include("Comments")
                                      .Include("User")
                                      .Where(subject => subject.Id == id)
                                      .First();
            subject.Categ = GetAllCategories();

            if (subject.UserId.ToString() == userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Administrator"))
            {
                return View(subject);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int id, Subject updatedSubject)
        {
            var subject = db.Subjects.Include("Comments")
                                     .Include("User")
                                     .Where(subject => subject.Id == id)
                                     .First();

            if (ModelState.IsValid)
            {
                if (subject.UserId.ToString() == userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Administrator"))
                {
                    var old_subject_category = subject.CategoryId; ;
                    subject.Title = updatedSubject.Title;
                    subject.Text = updatedSubject.Text;
                    subject.CategoryId = updatedSubject.CategoryId;

                    db.SaveChanges();

                    var category_old = db.Categories.Find(old_subject_category);
                    var category_new = db.Categories.Find(updatedSubject.CategoryId);

                    category_old.Subjects.Remove(subject);
                    category_new.Subjects.Add(subject);

                    db.SaveChanges();

                    return RedirectToAction("Index", new { id = updatedSubject.CategoryId, sortOrder = "desc", sortType = "time" });
                }
                else
                {
                    return RedirectToAction("Index", new { id = subject.CategoryId, sortOrder = "desc", sortType = "time" });
                }
            }
            else
            {
                updatedSubject.Categ = GetAllCategories();
                ViewBag.Subject = updatedSubject;
                ViewBag.UserId = userManager.GetUserId(User);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var subject = db.Subjects.Include("Comments")
                                     .Include("User")
                                     .Where(subject => subject.Id == id)
                                     .First();

            if (subject.UserId.ToString() == userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Administator"))
            {
                foreach (var comment in subject.Comments)
                {
                    db.Comments.Remove(comment);
                }
                db.SaveChanges();

                db.Subjects.Remove(subject);
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { id = subject.CategoryId });
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = db.Categories.ToList();

            foreach(var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.Id.ToString();
                listItem.Text = category.Name.ToString();
                selectList.Add(listItem);
            }

            return selectList;
        }
    }
}
