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

      

        public async Task<ActionResult> ThemeIndex(int id, int? page)
        {
            ApplicationUser user = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = await Usermanager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            ViewBag.User = user;
            ViewBag.Banned = UserBanState(user);
            ViewBag.page = page ?? 1;
            Theme theme = await DBContext.Themes.FindAsync(id);
            if(theme == null)
            {
                return View("Error");
            }
            return View(theme);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostSave(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Theme theme = await DBContext.Themes.FindAsync(model.ID);
                ApplicationUser user = await Usermanager.FindByNameAsync(model.UserName);
                if (theme == null || user == null || UserBanState(user))
                {
                    return View("Error");
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


            return RedirectToAction("ThemeIndex", new { id = model.ID, page = model.Page });
        }

        private bool UserBanState(ApplicationUser user)
        {
           
            if(user.BannedDate == null)
            {
                return false;
            } else 
            {
                return user.BannedDate > DateTime.Now;  
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult> EditPost(int postid, int? page)
        {
            ApplicationUser user = await Usermanager.FindByNameAsync(HttpContext.User.Identity.Name);
            Post post = await DBContext.Posts.FindAsync(postid);
            if (user == null || post == null || 
                (user.UserName != post.Owner.UserName && !HttpContext.User.IsInRole("Moderators")) || UserBanState(user))
            {
                return View("Error");
            }

            ViewBag.page = page ?? 1;
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EditPost(PostViewModel editedpost)
        {
            Post post = await DBContext.Posts.FindAsync(editedpost.ID);
            post.EditedDate = DateTime.Now;
            post.Text = editedpost.PostText;
            await DBContext.SaveChangesAsync();
            return RedirectToAction("ThemeIndex", new { id = post.Theme.ID, page = editedpost.Page});
        }

       

    }
}