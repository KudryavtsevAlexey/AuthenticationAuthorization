using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationAuthentication.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string UserName{ get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
