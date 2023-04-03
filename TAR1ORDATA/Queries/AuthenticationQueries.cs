using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class AuthenticationQueries
    {
        public static readonly string sqlGetUserByIdAndPass = "select userid,name,RTRIM(workgroupid)[workgroupid] from secuser where userid=@userid and loginpwd=@pass;";
        public static readonly string sqlGetRolesByUserId = "select rtrim(usr.workgroupid) [rolecode], rtrim(wgrp.description) [roledescription]  " +
                                                            "from secuser usr inner join secworkgroup wgrp on usr.workgroupid=wgrp.workgroupid " +
                                                            "where userid=@userid;";
    }
}