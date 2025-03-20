using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.DataModel
{
    public class PaymentViewModel
    {
        public PaymentHeaderModel PaymentHeader { get; set; }
        public List<PaymentDetailModel> PaymentDetails { get; set; }
        public List<PaymentApplyModel> PaymentApplies { get; set; }
    }
}