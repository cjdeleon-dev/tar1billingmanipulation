using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.DataService.UserLoginService;

namespace TAR1ORMAN.SiteRoleProvider
{
    public class SiteRole : RoleProvider
    {
        IUserLoginService iul;
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            iul = new UserLoginService();

            List<RoleModel> lstrm = new List<RoleModel>();

            lstrm = iul.GetRolesByUserId(username);

            string[] result = { lstrm[0].RoleCode };

            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
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