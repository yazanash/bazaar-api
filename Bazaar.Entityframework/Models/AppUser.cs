using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}
