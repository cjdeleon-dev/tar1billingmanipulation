using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class HomeQueries
    {
        public static readonly string sqlListOfORInEightAddition = "select ornumber, LEFT(ornumber,1) + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 order by ornumber";

        public static readonly string sqlListOfORInSevenAddition = "select ornumber, LEFT(ornumber,1) + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 order by ornumber";

        public static readonly string sqlListOfORInEightSubtraction = "select ornumber, LEFT(ornumber,1) + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor, payee, consumerid, trxdate  " +
                                                             "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 order by ornumber";

        public static readonly string sqlListOfORInSevenSubtraction = "select ornumber, LEFT(ornumber,1) + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 order by ornumber";



        public static readonly string sqlSetNewORByRangeEightDigAdd = "DECLARE @OLDPREFIX VARCHAR(1); " +
                                                                  "SET @OLDPREFIX = LEFT(@ORFROM,1); " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, '^' + CAST((CAST(REPLACE(ornumber,@OLDPREFIX,'') AS INT) + @ADD) AS VARCHAR(9)) newor " +
                                                                  "     from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 " +
                                                                  ") s on t.ornumber=s.ornumber; " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, @OLDPREFIX + REPLACE(ornumber,'^','') newor " +
                                                                  "     from arspaytrxhdr where ornumber between '^' + CAST((CAST(REPLACE(@ORFROM,@OLDPREFIX,'') AS INT) + @ADD) AS VARCHAR(9)) " +
                                                                  "     and '^' + CAST((CAST(REPLACE(@ORTO,@OLDPREFIX,'') AS INT) + @ADD) AS VARCHAR(9)) and len(rtrim(ornumber))=8 " +
                                                                  ") s on t.ornumber=s.ornumber;";

        public static readonly string sqlSetNewORByRangeSevenDigAdd = "DECLARE @OLDPREFIX VARCHAR(1); " +
                                                                  "SET @OLDPREFIX = LEFT(@ORFROM,1); " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, '^' + CAST((CAST(REPLACE(ornumber,@OLDPREFIX,'') AS INT) + @ADD) AS VARCHAR(9)) newor " +
                                                                  "     from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 " +
                                                                  ") s on t.ornumber=s.ornumber; " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, @OLDPREFIX + REPLACE(ornumber,'^','') newor " +
                                                                  "     from arspaytrxhdr where ornumber between '^' + CAST((CAST(REPLACE(@ORFROM,@OLDPREFIX,'') AS INT) + @ADD) AS VARCHAR(9)) " +
                                                                  "     and '^' + CAST((CAST(REPLACE(@ORTO,@OLDPREFIX,'') AS INT) +                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              @ADD) AS VARCHAR(9)) and len(rtrim(ornumber))=7 " +
                                                                  ") s on t.ornumber=s.ornumber;";
        //subtraction
        public static readonly string sqlSetNewORByRangeEightDigSubtract = "DECLARE @OLDPREFIX VARCHAR(1); " +
                                                                  "SET @OLDPREFIX = LEFT(@ORFROM,1); " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, '^' + CAST((CAST(REPLACE(ornumber,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) newor " +
                                                                  "     from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 " +
                                                                  ") s on t.ornumber=s.ornumber; " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, @OLDPREFIX + REPLACE(ornumber,'^','') newor " +
                                                                  "     from arspaytrxhdr where ornumber between '^' + CAST((CAST(REPLACE(@ORFROM,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) " +
                                                                  "     and '^' + CAST((CAST(REPLACE(@ORTO,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) and len(rtrim(ornumber))=8 " +
                                                                  ") s on t.ornumber=s.ornumber;";

        public static readonly string sqlSetNewORByRangeSevenDigSubtract = "DECLARE @OLDPREFIX VARCHAR(1); " +
                                                                  "SET @OLDPREFIX = LEFT(@ORFROM,1); " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, '^' + CAST((CAST(REPLACE(ornumber,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) newor " +
                                                                  "     from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 " +
                                                                  ") s on t.ornumber=s.ornumber; " +
                                                                  "update t " +
                                                                  "set t.ornumber=s.newor " +
                                                                  "from arspaytrxhdr t inner join " +
                                                                  "( " +
                                                                  "     select ornumber, @OLDPREFIX + REPLACE(ornumber,'^','') newor " +
                                                                  "     from arspaytrxhdr where ornumber between '^' + CAST((CAST(REPLACE(@ORFROM,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) " +
                                                                  "     and '^' + CAST((CAST(REPLACE(@ORTO,@OLDPREFIX,'') AS INT) - @SUBTRACT) AS VARCHAR(9)) and len(rtrim(ornumber))=7 " +
                                                                  ") s on t.ornumber=s.ornumber;";

       

        public static readonly string sqlListNewPrefixOfORRangeEightNone = "select ornumber, @NEWPREFIX + RTRIM(right(ornumber,9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 order by ornumber";
        public static readonly string sqlListNewPrefixOfORRangeSevenNone = "select ornumber, @NEWPREFIX + RTRIM(right(ornumber,9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 order by ornumber";
        public static readonly string sqlListNewPrefixOfORRangeEightAddition = "select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 order by ornumber";
        public static readonly string sqlListNewPrefixOfORRangeEightSubtraction = "select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 order by ornumber";
        public static readonly string sqlListNewPrefixOfORRangeSevenAddition = "select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 order by ornumber";
        public static readonly string sqlListNewPrefixOfORRangeSevenSubtraction = "select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor, payee, consumerid, trxdate " +
                                                              "from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 order by ornumber";



        public static readonly string sqlSetNewPrefixOfORRangeEightNone = "update t " +
                                                                          "set t.ornumber=s.newor " +
                                                                          "from arspaytrxhdr t inner join " +
                                                                          "( " +
                                                                          "  select ornumber, @NEWPREFIX + RTRIM(right(ornumber,9)) newor " +
                                                                          "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 " +
                                                                          ") s on t.ornumber=s.ornumber; ";
        public static readonly string sqlSetNewPrefixOfORRangeSevenNone = "update t " +
                                                                          "set t.ornumber=s.newor " +
                                                                          "from arspaytrxhdr t inner join " +
                                                                          "( " +
                                                                          "  select ornumber, @NEWPREFIX + RTRIM(right(ornumber,9)) newor " +
                                                                          "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 " +
                                                                          ") s on t.ornumber=s.ornumber; ";
        public static readonly string sqlSetNewPrefixOfORRangeEightAddition = "update t " +
                                                                              "set t.ornumber=s.newor " +
                                                                              "from arspaytrxhdr t inner join " +
                                                                              "( " +
                                                                              "  select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor " +
                                                                              "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 " +
                                                                              ") s on t.ornumber=s.ornumber; ";
        public static readonly string sqlSetNewPrefixOfORRangeSevenAddition = "update t " +
                                                                              "set t.ornumber=s.newor " +
                                                                              "from arspaytrxhdr t inner join " +
                                                                              "( " +
                                                                              "  select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) + @ADDEND) AS VARCHAR(9)) newor " +
                                                                              "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 " +
                                                                              ") s on t.ornumber=s.ornumber; ";
        public static readonly string sqlSetNewPrefixOfORRangeEightSubtraction = "update t " +
                                                                                 "set t.ornumber=s.newor " +
                                                                                 "from arspaytrxhdr t inner join " +
                                                                                 "( " +
                                                                                 "  select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor " +
                                                                                 "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=8 " +
                                                                                 ") s on t.ornumber=s.ornumber; ";
        public static readonly string sqlSetNewPrefixOfORRangeSevenSubtraction = "update t " +
                                                                                 "set t.ornumber=s.newor " +
                                                                                 "from arspaytrxhdr t inner join " +
                                                                                 "( " +
                                                                                 "  select ornumber, @NEWPREFIX + CAST((CAST(RTRIM(right(ornumber,9)) AS INT) - @SUBTRACT) AS VARCHAR(9)) newor " +
                                                                                 "  from arspaytrxhdr where ornumber between @ORFROM and @ORTO and len(rtrim(ornumber))=7 " +
                                                                                 ") s on t.ornumber=s.ornumber; ";

        public static readonly string sqlAllSecUsers = "select userid, name from secuser";
    }
}