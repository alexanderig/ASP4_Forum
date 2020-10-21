using ASP4_Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

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
    }
}