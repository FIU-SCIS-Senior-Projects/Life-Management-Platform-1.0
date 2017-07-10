using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LifeManagement.Models
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity ident, List<string> roles, int someCustomProperty1, string someCustomProperty2)
        {
            this.identity = ident;
            this.roles = roles;
            this.someCustomProperty1 = someCustomProperty1;
            this.someCustomProperty2 = someCustomProperty2;
        }

        IIdentity identity;

        public IIdentity Identity
        {
            get { return identity; }
        }

        private List<string> roles;

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        private int someCustomProperty1;

        public int SomeCustomProperty1
        {
            get { return someCustomProperty1; }
        }

        private string someCustomProperty2;

        public string SomeCustomProperty2
        {
            get { return someCustomProperty2; }
        }
    }

    public class Authorization
    {
    }
}