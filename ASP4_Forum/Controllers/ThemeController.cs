using ASP4_Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ASP4_Forum.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using System.Web.WebPages;
using System.Web.Helpers;

namespace ASP4_Forum.Controllers
{
    public class ThemeController : Controller
    {
        ApplicationDbContext _context;
        ApplicationUserManager _umanager;
        public ApplicationDbContext DBContext
        {
            get
            {
                return _context ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _context = value;
            }
        }

        public ApplicationUserManager Usermanager
        {
            get
            {
                return _umanager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _umanager = value;
            }
        }

        public ThemeController() { }

        // GET: Theme
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Index(int id)
        {
            Theme theme = await DBContext.Themes.FindAsync(id);
            return View(theme);
        }

        [HttpPost]
        public async Task<JsonResult> PostSave(PostViewModel model)
        {
            if(ModelState.IsValid)
            {
                Theme theme = await DBContext.Themes.FindAsync(model.ThemeID);
                ApplicationUser user = await Usermanager.FindByNameAsync(model.UserName);
                if (theme == null || user == null)
                {
                    return Json("error");
                }
                Post post = new Post();
                post.Owner = user;
                post.IsBanned = false;
                post.Theme = theme;
                post.Text = model.PostText;
                post.Date = DateTime.Now;
                post.EditedDate = DateTime.Now;
                DBContext.Posts.Add(post);
                //DBContext.Entry(post).State = System.Data.Entity.EntityState.Added; 
                await DBContext.SaveChangesAsync();
            }
            
            
            return Json(model.PostText);
        }
    }
}