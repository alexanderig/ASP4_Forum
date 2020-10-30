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

namespace ASP4_Forum.Controllers
{
    public class ThemeController : Controller
    {
        ApplicationDbContext _context;

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
            int a = 10;
            string aa = Request.Form["themeid"];
            string tt = "<p>Hello</p>";
            string cc = Request.Form["username"];
            string bb = Request.Form["posttext"].As<string>();
           
            
            //Theme theme = await DBContext.Themes.FindAsync(postmodel.ThemeID);
            return Json(model.PostText);
        }
    }
}