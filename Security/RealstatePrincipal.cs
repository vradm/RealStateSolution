using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace RealState.Security
{
    public class RealstatePrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get; private set;
        }
        public bool IsInRole(string Role)
        {
            if (Role.Contains(','))
            {
                foreach (var item in Role.Split(','))
                {
                    if (Roles.Any(x => x.Contains(item)))
                    {
                        return true;
                    }
                }
            }
            return Roles.Any(x => x.Contains(Role));



        }
        public RealstatePrincipal(string UserName)
        {
            this.Identity = new GenericIdentity(UserName);
        }
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
        public string EmailId { get; set; }

        public class PrincipalSerializeModel
        {
            public long UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string[] Roles { get; set; }
            public string EmailId { get; set; }

        }
    }
}