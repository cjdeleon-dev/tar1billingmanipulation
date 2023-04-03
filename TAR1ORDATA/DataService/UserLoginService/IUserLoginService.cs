using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.UserLoginService
{
    public interface IUserLoginService
    {
        bool GetUserByIdAndPass(string userid, string pass);
        List<RoleModel> GetRolesByUserId(string userid);
    }
}
