using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.DTDWithCurrBillAccess;
using TAR1ORDATA.DataModel;

namespace TAR1ORDATA.DataService.DTDWithCurrBillService
{
    public class DTDWithCurrBillService : IDTDWithCurrBillService
    {
        IDTDWithCurrBillAccess iwcba;

        public DTDWithCurrBillService()
        {
            iwcba = new DTDWithCurrBillAccess();
        }

        public List<DTDWithCurrentBillModel> GetAllDTDWithCurrBills()
        {
            return iwcba.GetAllDTDWithCurrBills();
        }
    }
}