using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Remoting.Channels;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP4_Forum.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Themes = new HashSet<Theme>();
            Posts = new HashSet<Post>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string AvatarURL { get; set; }
        public virtual City City {get;set;}
        public virtual Country Country { get; set; }
        //public virtual Theme Theme { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Theme> Themes { get; set; }
        [InverseProperty("Curator")]
        public virtual ICollection<Theme> ThemesCurator { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public DateTime? BannedDate { get; set; }

    }

   
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Partition> Partitions { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
    }


}

