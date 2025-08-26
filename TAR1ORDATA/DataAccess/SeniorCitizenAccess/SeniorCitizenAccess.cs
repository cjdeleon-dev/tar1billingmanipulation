using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.SeniorCitizenAccess
{
    public class SeniorCitizenAccess : ConnectionAccess, ISeniorCitizenAccess
    {
        public List<SeniorCitizenModel> GetAllSeniorCitizens()
        {
            List<SeniorCitizenModel> lstscm = new List<SeniorCitizenModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = "select consumerid, name, address, datesc [applieddate], datescexp [expirydate] " +
                                               "from arsconsumer " +
                                               "where statusid = 'A' and scflag = 'T';";

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string dateApp = string.Empty;
                            string dateExp = string.Empty;

                            if (dr["applieddate"] != DBNull.Value)
                                dateApp = Convert.ToDateTime(dr["applieddate"]).ToString("MM/dd/yyyy");

                            if (dr["expirydate"] != DBNull.Value)
                                dateExp = Convert.ToDateTime(dr["expirydate"]).ToString("MM/dd/yyyy");

                            lstscm.Add(new SeniorCitizenModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                Name = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                AppliedDate = dateApp,
                                ExpiryDate = dateExp
                            });
                        }
                    }
                    else
                    {
                        lstscm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstscm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstscm;
        }
    }
}