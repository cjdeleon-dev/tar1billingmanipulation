using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class EmpPowerBillModel
    {
        public string EmployeeName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public int NumOfMonths { get; set; }
        public double PowerBill { get; set; }
        public double VAT { get; set; }
        public double Surcharge { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }
}