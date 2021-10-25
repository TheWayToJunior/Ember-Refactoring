using Ember.Infrastructure.Data.Entitys;
using Microsoft.AspNetCore.Identity;

namespace Ember.Infrastructure.Data.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}
