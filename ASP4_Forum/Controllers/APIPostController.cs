using ASP4_Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;

namespace ASP4_Forum
{
    [Authorize]
    [RoutePrefix("api/apiPost")]
    public class APIPostController : ApiController
    {
       
        private ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;
       
        public ApplicationDbContext DBContext
        {
            get
            {
                return _dbContext ?? Request.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [HttpPatch]       
        public async Task<JsonResult<string>> BlockingPost(int id)
        {
            Post post = await DBContext.Posts.FindAsync(id);
            if (post == null)
            {
                return Json("Not found");
            }
            post.IsBanned = post.IsBanned ? false : true;
            await DBContext.SaveChangesAsync();
            if (post.IsBanned)
            {
                return Json("Blocked");
            }
            else
            {
                return Json("Unblocked");
            }

        }

      
        [HttpDelete]       
        public async Task<IHttpActionResult> DeletePost(int id)
        {
            Post post = await DBContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            DBContext.Posts.Remove(post);
            await DBContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> BanUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            DateTime bandate = user.BannedDate ?? DateTime.Now;
            user.BannedDate = bandate.AddDays(1);
            await UserManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPut]
        public async Task<IHttpActionResult> ResetBan(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.BannedDate = DateTime.Now;
            await UserManager.UpdateAsync(user);
            return Ok();
        }

    }
}