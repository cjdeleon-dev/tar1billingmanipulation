using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.UserLoginAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.UserLoginService
{
    public class UserLoginService : IUserLoginService
    {
        IUserLoginAccess iulaccess;

        public UserLoginService()
        {
            this.iulaccess = new UserLoginAccess();
        }

        public List<RoleModel> GetRolesByUserId(string userid)
        {
            return iulaccess.GetRolesByUserId(userid);
        }

        public bool GetUserByIdAndPass(string userid, string pass)
        {
            return iulaccess.GetUserByIdAndPass(userid, pass);
        }
    }
}