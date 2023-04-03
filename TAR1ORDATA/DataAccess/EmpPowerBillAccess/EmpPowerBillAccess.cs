using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Queries;

namespace TAR1ORDATA.DataAccess.EmpPowerBillAccess
{
    public class EmpPowerBillAccess : ConnectionAccess, IEmpPowerBillAccess
    {
        public List<EmpPowerBillModel> GetAllEmpPowerBills()
        {
            List<EmpPowerBillModel> lstepbm = new List<EmpPowerBillModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = EmpPowerBillQueries.sqlGetEmpPowerBills;

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstepbm.Add(new EmpPowerBillModel
                            {
                                EmployeeName = dr["EMP NAME"].ToString(),
                                AccountNo = dr["ACCOUNT NO"].ToString(),
                                AccountName = dr["ACCOUNT NAME"].ToString(),
                                NumOfMonths = Convert.ToInt32(dr["NUMOFMONTHS"]),
                                PowerBill = Convert.ToDouble (dr["POWER BILL"]),
                                VAT = Convert.ToDouble(dr["VAT"]),
                                Surcharge = Convert.ToDouble(dr["SURCHARGE"]),
                                Total = Convert.ToDouble(dr["TOTAL"]),
                                Status = dr["STATUS"].ToString()
                            });
                        }
                    }
                    else
                    {
                        lstepbm = null;
                    }
                }
                catch (Exception ex)
                {
                    lstepbm = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstepbm;
        }
    }
}