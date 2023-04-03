using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataAccess.DTDWithCurrBillAccess
{
    public class DTDWithCurrBillAccess : ConnectionAccess, IDTDWithCurrBillAccess
    {
        public List<DTDWithCurrentBillModel> GetAllDTDWithCurrBills()
        {
            List<DTDWithCurrentBillModel> lstdtdwcb = new List<DTDWithCurrentBillModel>();

            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                da.SelectCommand.Connection.Open();

                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = Queries.DTDWithCurrBillQueries.getAllDTDWithCurrBills;

                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            lstdtdwcb.Add(new DTDWithCurrentBillModel
                            {
                                AccountNo = dr["consumerid"].ToString(),
                                AccountName = dr["name"].ToString(),
                                Address = dr["address"].ToString(),
                                MeterNumber = dr["mtrserialno"].ToString(),
                                PoleId = dr["poleid"].ToString(),
                                Type = dr["type"].ToString(),
                                Amount = Convert.ToDouble(dr["amount"])
                            });
                        }
                    }
                    else
                    {
                        lstdtdwcb = null;
                    }
                }
                catch (Exception ex)
                {
                    lstdtdwcb = null;
                }
                finally
                {
                    da.SelectCommand.Connection.Close();
                }
            }

            return lstdtdwcb;
        }
    }
}