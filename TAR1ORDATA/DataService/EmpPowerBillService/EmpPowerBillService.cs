using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.EmpPowerBillAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.EmpPowerBillService
{
    public class EmpPowerBillService : IEmpPowerBillService
    {
        IEmpPowerBillAccess iepna;

        public EmpPowerBillService()
        {
            this.iepna = new EmpPowerBillAccess();
        }

        public List<EmpPowerBillModel> GetAllEmpPowerBills()
        {
            return iepna.GetAllEmpPowerBills();
        }
    }
}