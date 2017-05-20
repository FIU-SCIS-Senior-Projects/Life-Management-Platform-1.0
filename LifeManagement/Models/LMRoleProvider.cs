using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using LifeManagement.Models;
namespace LifeManagement.Models
{
    public class LMRoleProvider : RoleProvider
    {
       
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = {"guest"};
              SeniorDBEntities db = new SeniorDBEntities();
            var user = db.Users.Where(a => a.username.ToLower() == username.ToLower()).FirstOrDefault();
            if (user != null)
                return new string[] {user.Role.Name};

            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            SeniorDBEntities db = new SeniorDBEntities();
            var user = db.Users.Where(a => a.username.ToLower() == username.ToLower()).FirstOrDefault();
            if (user == null)
                return false;
            return user.Role.Name.ToLower() == roleName.ToLower();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}