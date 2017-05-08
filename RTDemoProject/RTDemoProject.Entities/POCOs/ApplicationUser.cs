using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RTDemoProject.Shared.Enums;

namespace RTDemoProject.Entities.POCOs
{
    public class ApplicationUser : IdentityUser, IObjectState
    {
        public virtual Employee Employee { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }

    public class ApplicationUserMap : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            HasKey(x => x.Id);
            Ignore(x => x.PhoneNumber);
            Ignore(x => x.PhoneNumberConfirmed);
        }
    }
}