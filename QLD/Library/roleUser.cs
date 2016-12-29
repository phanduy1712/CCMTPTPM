using System; 
using System.Linq; 
using System.Web.Security;
using QLD.Models;

namespace QLD.Library
{
    public class RoleUser : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var db = new Entities();
            //if (username == "Admin") { return new String[] { "Admin" }; }
            //var objRS = db.UserGroupDetails.Where(t => t.User.Account == username).Select(t => t.RoleGroupId).ToArray();
            //var userGroup = db.RoleGroupDetails.Where(t => objRS.Contains(t.RoleGroupId)).Select(t => t.Role.Code).Distinct().ToArray();  
            ////var userGroup = db.UserRoles.Where(t => t.User.Account == username).Select(t => t.Role.Code).Distinct().ToArray(); 

            //db.Dispose();
            //if (userGroup.Count() > 0)
            //{
            //    return userGroup;
            //}
            //else
            //{
            //    return new String[] { };
            //}
            return new String[] { "QL"};
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }

    }
}