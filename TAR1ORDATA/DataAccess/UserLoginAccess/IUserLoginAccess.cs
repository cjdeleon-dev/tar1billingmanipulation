using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.UserLoginAccess
{
    public interface IUserLoginAccess
    {
        bool GetUserByIdAndPass(string userid, string pass);
        List<RoleModel> GetRolesByUserId(string userid);
    }
}
