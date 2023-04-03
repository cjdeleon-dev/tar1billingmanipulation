using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAR1ORDATA.DataAccess.HomeAccess;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Enums;

namespace TAR1ORDATA.DataService.HomeService
{
    public class HomeService : IHomeService
    {
        IHomeAccess ihomeaccess;

        public HomeService()
        {
            this.ihomeaccess = new HomeAccess();
        }

        public bool isNewORVacant(string ORFrom, string ORTo, int AddDiffValue, bool isAddition)
        {
            return ihomeaccess.isNewORVacant(ORFrom, ORTo, AddDiffValue, isAddition);
        }

        public List<TransactionHeaderModel> ListNewPrefixOfORRange(TransactionSearchModel tsm)
        {
            return ihomeaccess.ListNewPrefixOfORRange(tsm);
        }

        public List<TransactionHeaderModel> ListOfORInAddition(TransactionSearchModel tsm)
        {
            return ihomeaccess.ListOfORInAddition(tsm);
        }

        public List<TransactionHeaderModel> ListOfORInSubtraction(TransactionSearchModel tsm)
        {
            return ihomeaccess.ListOfORInSubtraction(tsm);
        }

        public VMNewORByRange ProcessNewORByRangeAdd(string ORFrom, string ORTo, int Additional)
        {
            return ihomeaccess.ProcessNewORByRangeAdd(ORFrom, ORTo, Additional);
        }

        public VMNewORByRange ProcessNewORByRangeDiff(string ORFrom, string ORTo, int Difference)
        {
            return ihomeaccess.ProcessNewORByRangeDiff(ORFrom, ORTo, Difference);
        }

        public VMNewPrefixOfORRange ProcessNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0)
        {
            return ihomeaccess.ProcessNewPrefixOfORRange(ORFrom, ORTo, NewPrefix, Difference, Additional);
        }

        public bool SetNewORByRangeAdd(string ORFrom, string ORTo, int Additional)
        {
            return ihomeaccess.SetNewORByRangeAdd(ORFrom, ORTo, Additional);
        }

        public bool SetNewORByRangeDiff(string ORFrom, string ORTo, int Difference)
        {
            return ihomeaccess.SetNewORByRangeDiff(ORFrom, ORTo, Difference);
        }

        public bool SetNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0)
        {
            return ihomeaccess.SetNewPrefixOfORRange(ORFrom, ORTo, NewPrefix, Difference, Additional);
        }
    }
}