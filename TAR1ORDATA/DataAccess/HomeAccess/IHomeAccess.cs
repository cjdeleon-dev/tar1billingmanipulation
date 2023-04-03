using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAR1ORDATA.DataModel;
using TAR1ORDATA.Enums;

namespace TAR1ORDATA.DataAccess.HomeAccess
{
    public interface IHomeAccess
    {
        bool SetNewORByRangeDiff(string ORFrom, string ORTo, int Difference);
        bool SetNewORByRangeAdd(string ORFrom, string ORTo, int Additional);

        VMNewORByRange ProcessNewORByRangeAdd(string ORFrom, string ORTo, int Additional);
        VMNewORByRange ProcessNewORByRangeDiff(string ORFrom, string ORTo, int Difference);

        List<TransactionHeaderModel> ListOfORInAddition(TransactionSearchModel tsm);
        List<TransactionHeaderModel> ListOfORInSubtraction(TransactionSearchModel tsm);


        List<TransactionHeaderModel> ListNewPrefixOfORRange(TransactionSearchModel tsm);
        bool SetNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0);

        VMNewPrefixOfORRange ProcessNewPrefixOfORRange(string ORFrom, string ORTo, string NewPrefix, int Difference = 0, int Additional = 0);

        bool isNewORVacant(string ORFrom, string ORTo, int AddDiffValue, bool isAddition);
        
    }
}
