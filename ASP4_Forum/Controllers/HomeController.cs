using ASP4_Forum.Models;
using ASP4_Forum.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http;
//using System.Web.Http;
//using System.Web.Http;
//using System.Web.Http;
//using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
//using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace ASP4_Forum.Controllers
{
    [RequireHttps] //for google authorization
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ApplicationDbContext _context;

        public HomeController() { }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

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

        public ActionResult Index()
        {
            List<Section> sections = DBContext.Sections.Include(p => p.Partitions).ToList();
            return View(sections);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[System.Web.Http.Authorize(Roles = "Users")]
        //public ActionResult OtherAction()
        //{
        //    return View("Index", GetData("OtherAction"));
        //}

        public async Task<ActionResult> PartView(int partId)
        {
            Partition partition = await DBContext.Partitions.FindAsync(partId);
            if(partition == null)
            {
                return View("Error");
            }
            return View(partition);
        }


        public async Task<ActionResult> Partitions()
        {
            List<Partition> partitions = await DBContext.Partitions.ToListAsync();

            return View(partitions);
        }

        [HttpGet]
        public ActionResult CreatePartition()
        {
            ViewBag.ListSections = new SelectList(DBContext.Sections, "ID", "Name");
            return PartialView(new PartitionViewModel { Name = "", Section = null });
        }

        [HttpPost]
        public async Task<ActionResult> CreatePartition(PartitionViewModel vmpart)
        {
            if (ModelState.IsValid)
            {
                Section section = DBContext.Sections.Find(vmpart.SectionID);
                if (section == null)
                    return View("Error");
                Partition partition = new Partition { Name = vmpart.Name, Date = DateTime.Now, Section = section };
                DBContext.Partitions.Add(partition);
                await DBContext.SaveChangesAsync();
                return RedirectToAction("Partitions");
            }

            return PartialView(vmpart);
        }


        [HttpGet]
        public async Task<ActionResult> DelPartition(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Partition partition = await DBContext.Partitions.FindAsync(id);
            if (partition == null)
                return HttpNotFound();
            if (partition.Themes.Count > 0)
            {
                ModelState.AddModelError("", "Partition contains Themes");
            }

            return PartialView(partition);
        }

        [HttpPost, ActionName("DelPartition")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DelPartitionOk(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Partition partition = await DBContext.Partitions.FindAsync(id);
            if (partition == null)
            {
                return HttpNotFound();
            }
            if (partition.Themes.Count() > 0)
            {
                ModelState.AddModelError("", "Partition contains Themes");
                return PartialView(partition);
            }
            DBContext.Partitions.Remove(partition);
            await DBContext.SaveChangesAsync();
            //return RedirectToAction("Partitions");
            return Json("ok");
        }

        [HttpGet]
        public async Task<ActionResult> EditPartitionName(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Partition partition = await DBContext.Partitions.FindAsync(id);
            if (partition == null)
                return HttpNotFound();
            //ViewBag.ListSections = new SelectList(DBContext.Sections, "ID", "Name");
            return PartialView(partition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPartitionName(Partition editedpart)
        {
            if (ModelState.IsValid)
            {
                Partition partition = await DBContext.Partitions.FindAsync(editedpart.ID);
                if (partition != null)
                {
                    partition.Name = editedpart.Name;
                    DBContext.Entry(partition).State = EntityState.Modified;
                    await DBContext.SaveChangesAsync();
                    return RedirectToAction("Partitions");
                }
                return View("Error");
            }
            return PartialView(editedpart);
        }


        [HttpGet]
        public async Task<ActionResult> EditPartitionContent(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Partition partition = await DBContext.Partitions.FindAsync(id);
            // string[] memberIDs = section.Partitions.Where(p => p.ID;
            try
            {
                IEnumerable<Theme> members = partition.Themes;// Where(x => memberIDs.Any(y => y == x.Id));

                IEnumerable<Theme> nonMembers = DBContext.Themes.ToList().Except(members).ToList();// UserManager.Users.Except(members);

                ViewBag.Partitions = await DBContext.Partitions.ToListAsync();
                return PartialView(new PartitionEditModel
                {
                    Partition = partition,
                    Members = members,
                    NonMembers = nonMembers
                });
            }
            catch (Exception ex)
            {
                return View("Error");
            }


        }

        [HttpPost]
        public async Task<ActionResult> EditPartitionContent(SectionModificationModel model)
        {

            if (ModelState.IsValid)
            {
                Partition partition = await DBContext.Partitions.FindAsync(model.ID);
                foreach (int themeId in model.IdsToAdd ?? new int[] { })
                {

                    Theme theme = await DBContext.Themes.FindAsync(themeId);
                    theme.Partition = partition;
                    DBContext.Entry(theme).State = EntityState.Modified;
                }
                if (model.IDChild != null)
                {
                    for (int i = 0; i < model.IDChild.Length; i++)
                    {
                        Theme theme = await DBContext.Themes.FindAsync(model.IDChild[i]);
                        Partition parent = await DBContext.Partitions.FindAsync(model.IDParent[i]);
                        if (theme != null && parent != null && !parent.Equals(partition))
                        {
                            theme.Partition = parent;
                            DBContext.Entry(theme).State = EntityState.Modified;
                        }

                    }
                }

                await DBContext.SaveChangesAsync();
                return RedirectToAction("Partitions");

            }
            return View("Error", new string[] { "Partition not found" });
        }


        public async Task<ActionResult> Sections()
        {
            List<Section> sections = await DBContext.Sections.ToListAsync();
            return View(sections);
        }

        [HttpGet]
        public ActionResult CreateSection()
        {
            return PartialView(new Section { Name = "", Date = DateTime.Now, Partitions = null });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSection(Section section)
        {
            if (ModelState.IsValid)
            {
                section.Date = DateTime.Now;
                DBContext.Sections.Add(section);
                await DBContext.SaveChangesAsync();
                return RedirectToAction("Sections");
            }

            return PartialView(section);
        }


        [HttpGet]
        public async Task<ActionResult> EditSectionName(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Section section = await DBContext.Sections.FindAsync(id);
            if (section == null)
                return HttpNotFound();

            return PartialView(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSectionName(Section editedsection)
        {
            if (ModelState.IsValid)
            {
                Section section = await DBContext.Sections.FindAsync(editedsection.ID);
                if (section != null)
                {
                    section.Name = editedsection.Name;
                    DBContext.Entry(section).State = EntityState.Modified;
                    await DBContext.SaveChangesAsync();
                    return RedirectToAction("Sections");
                }
                return View("Error");
            }
            return PartialView(editedsection);
        }



        [HttpGet]
        public async Task<ActionResult> DelSection(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Section section = await DBContext.Sections.FindAsync(id);
            if (section == null)
                return HttpNotFound();

            if (section.Partitions.Count() > 0)
            {
                ModelState.AddModelError("", "Section contains Partitions");
                //   SectionModel sectionModel = new SectionModel { ID = section.ID, Date = section.Date, Name = section.Name };
                // return PartialView(sectionModel);
            }

            //Section sectionModel = new SectionModel { ID = section.ID, Date = section.Date, Name = section.Name };
            return PartialView(section);
        }

        [HttpPost, ActionName("DelSection")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DelSectionOk(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Section section = await DBContext.Sections.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            if (section.Partitions.Count() > 0)
            {
                ModelState.AddModelError("", "Section contains Partitions");
                //SectionModel sectionModel = new SectionModel { ID = section.ID, Date = section.Date, Name = section.Name };
                return PartialView(section);
            }
            DBContext.Sections.Remove(section);
            await DBContext.SaveChangesAsync();
            return Json("ok");
            //return RedirectToAction("Sections");
        }

        [HttpGet]
        public async Task<ActionResult> EditSectionContent(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Section section = await DBContext.Sections.FindAsync(id);
            // string[] memberIDs = section.Partitions.Where(p => p.ID;
            try
            {
                IEnumerable<Partition> members = section.Partitions;// Where(x => memberIDs.Any(y => y == x.Id));

                IEnumerable<Partition> nonMembers = DBContext.Partitions.ToList().Except(members).ToList();// UserManager.Users.Except(members);

                ViewBag.Sections = await DBContext.Sections.ToListAsync();
                return PartialView(new SectionEditModel
                {
                    Section = section,
                    Members = members,
                    NonMembers = nonMembers
                });
            }
            catch (Exception ex)
            {
                return View("Error");
            }


        }

        [HttpPost]
        public async Task<ActionResult> EditSectionContent(SectionModificationModel model)
        {

            if (ModelState.IsValid)
            {
                Section section = await DBContext.Sections.FindAsync(model.ID);
                foreach (int partId in model.IdsToAdd ?? new int[] { })
                {

                    Partition partition = await DBContext.Partitions.FindAsync(partId);
                    partition.Section = section;
                    DBContext.Entry(partition).State = EntityState.Modified;
                }
                if (model.IDChild != null)
                {
                    for (int i = 0; i < model.IDChild.Length; i++)
                    {
                        Partition partition = await DBContext.Partitions.FindAsync(model.IDChild[i]);
                        Section parent = await DBContext.Sections.FindAsync(model.IDParent[i]);
                        if (partition != null && parent != null && !parent.Equals(section))
                        {
                            partition.Section = parent;
                            DBContext.Entry(partition).State = EntityState.Modified;
                        }

                    }
                }

                await DBContext.SaveChangesAsync();
                return RedirectToAction("Sections");

            }
            return View("Error", new string[] { "Section not found" });
        }

        /// <summary>
        /// ////////////
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>



        public async Task<ActionResult> Themes()
        {
            List<Theme> themes = await DBContext.Themes.ToListAsync();
            return View(themes);
        }

        [HttpGet]
        public ActionResult CreateTheme(int? parentid)
        {
            
            // return PartialView(new Theme { Name = "", Date = DateTime.Now, Partitions = null });
            ViewBag.ListPartitions = parentid == null ? new SelectList(DBContext.Partitions, "ID", "Name") 
                : new SelectList(DBContext.Partitions.Where(i => i.ID == parentid), "ID", "Name");
            if(parentid != null)
            {
                return PartialView("CreateThemefromIndex", new ThemeViewModel { Name = "", Partition = null });
            } else
            {
                return PartialView(new ThemeViewModel { Name = "", Partition = null });
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> CreateTheme(ThemeViewModel thememodel, bool isindex)
        {
            if (ModelState.IsValid)
            {
                //using teheme class as model and ID used for Partition ID just neglete creating new vm class
                Partition partition = DBContext.Partitions.Find(thememodel.PartitionID);
                if (partition == null)
                    return View("Error");
                ApplicationUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    Theme theme = new Theme { Name = thememodel.Name, Date = DateTime.Now, Partition = partition, Creator = user };
                    DBContext.Themes.Add(theme);
                    await DBContext.SaveChangesAsync();
                }
                if (isindex)
                {
                    return RedirectToAction("Index");
                } else
                {
                    return RedirectToAction("Themes");
                }
                
            }

            return PartialView(thememodel);
        }


        [HttpGet]
        public async Task<ActionResult> DelTheme(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Theme theme = await DBContext.Themes.FindAsync(id);
            if (theme == null)
                return HttpNotFound();
            if (theme.Posts.Count > 0)
            {
                ModelState.AddModelError("", $"Partition contains {theme.Posts.Count} Posts");
            }

            return PartialView(theme);
        }

        [HttpPost, ActionName("DelTheme")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DelThemeOk(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Theme theme = await DBContext.Themes.FindAsync(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            if (theme.Posts.Count() > 0)
            {
                ModelState.AddModelError("", $"Theme contains {theme.Posts.Count} Posts");
                return PartialView(theme);
            }
            DBContext.Themes.Remove(theme);
            await DBContext.SaveChangesAsync();
            return Json("ok");
        }

        [HttpGet]
        public async Task<ActionResult> EditThemeName(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Theme theme = await DBContext.Themes.FindAsync(id);
            if (theme == null)
                return HttpNotFound();
            return PartialView(theme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditThemeName(Theme editedtheme)
        {
            if (ModelState.IsValid)
            {
                Theme theme = await DBContext.Themes.FindAsync(editedtheme.ID);
                if (theme != null)
                {
                    theme.Name = editedtheme.Name;
                    DBContext.Entry(theme).State = EntityState.Modified;
                    await DBContext.SaveChangesAsync();
                    return RedirectToAction("Themes");
                }
                return View("Error");
            }
            return PartialView(editedtheme);
        }

        [HttpGet]
        public async Task<ActionResult> EditThemeContent(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Theme theme = await DBContext.Themes.FindAsync(id);
            // string[] memberIDs = section.Partitions.Where(p => p.ID;
            try
            {
                //IEnumerable<Theme> members = ; //theme.Posts .Partitions;// Where(x => memberIDs.Any(y => y == x.Id));
                // IEnumerable<Theme> members = DBContext.Themes.Where(x => x.ID == theme.ID);

                // IEnumerable<Theme> nonMembers = DBContext.Themes.ToList().Except(members).ToList();// UserManager.Users.Except(members);

                ViewBag.Themes = await DBContext.Themes.ToListAsync();
                return PartialView(new ThemeEditModel
                {
                    Theme = theme,
                    Posts = theme.Posts.Count
                    // Members = members,
                    // NonMembers = nonMembers
                });
            }
            catch (Exception ex)
            {
                return View("Error");
            }


        }

        [HttpPost]
        public async Task<ActionResult> EditThemeContent(SectionModificationModel model)
        {

            if (ModelState.IsValid)
            {
                Theme owner = await DBContext.Themes.FindAsync(model.ID);

                if (model.IDChild != null && model.IDParent != null)
                {
                    //Theme owner = await DBContext.Themes.FindAsync(model.IDChild[0]);
                    Theme newparent = await DBContext.Themes.FindAsync(model.IDParent[0]);
                    if (owner != null && newparent != null && !newparent.Equals(owner))
                    {


                        for (int i = owner.Posts.Count - 1; i >= 0; i--)
                        {
                            owner.Posts.ElementAt(i).Theme = newparent; 
                            
                            //DBContext.Entry(post).State = EntityState.Modified;
                        }


                    }

                }

                await DBContext.SaveChangesAsync();
                return RedirectToAction("Themes");

            }
            return View("Error", new string[] { "Theme not found" });
        }




        ////////////////////////

        //[AcceptVerbs("Get", "Post")]
        public JsonResult CheckSection(string name)
        {
            Section section = DBContext.Sections.Where(c => c.Name.ToString().ToLower() == name.ToLower()).FirstOrDefault();
            if (section != null)
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult CheckSectionChildren(int id)
        //{
        //    Section section = DBContext.Sections.Find(id);
        //    if (section != null && section.Partitions.Count() > 0)
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    else
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult CheckPartition(string name)
        {
            Partition partition = DBContext.Partitions.Where(c => c.Name.ToString().ToLower() == name.ToLower()).FirstOrDefault();
            if (partition != null)
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckTheme(string name)
        {
            Theme theme = DBContext.Themes.Where(c => c.Name.ToString().ToLower() == name.ToLower()).FirstOrDefault();
            if (theme != null)
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }



        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated?", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("AuthenticationType", HttpContext.User.Identity.AuthenticationType);
            dict.Add("Is in role Users?", HttpContext.User.IsInRole("Users"));
            return dict;
        }


    }
}