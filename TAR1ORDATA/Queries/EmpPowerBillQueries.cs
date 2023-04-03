using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class EmpPowerBillQueries
    {
        public static readonly string sqlGetEmpPowerBills = "SELECT tblEmp.empname [EMP NAME],tblEmpCareOff.accountno [ACCOUNT NO],arsconsumer.[name] [ACCOUNT NAME], " +
    "ISNULL(src.NUMOFMONTHS,0) [NUMOFMONTHS],ROUND(ISNULL(src.EB,0),2) [POWER BILL],ROUND(ISNULL(src.VAT,0),2) [VAT],ROUND(ISNULL(src.SURCHARGE,0),2) [SURCHARGE], " +
    "ROUND(ISNULL(src.BALANCE,0),2) [TOTAL],arsconsumer.statusid [STATUS] " +
    "FROM tblEmp left join tblEmpCareOff " +
    "on tblEmp.id=tblEmpCareOff.empid " +
    "left join " +
    "( " +
    "	SELECT consumerid [ACCOUNT NO], [name] [CONSUMER NAME], SUM(SumEB) + SUM(SumVAT) + SUM(Surcharge) [BALANCE], SUM(SumEB) [EB], SUM(SumVAT) [VAT], SUM(Surcharge) [SURCHARGE], [address][ADDRESS], arrearctr [NUMOFMONTHS] " +
    "	FROM " +
    "	( " +
    "		SELECT a.consumerid,a.name,a.[address],SumEB,SumVAT,ROUND((CAST((((CAST(seq AS float)+Cast(ISNULL(c.ADDEND,0) AS float)) * CAST(scmult AS float))/100.00) AS FLOAT) * a.SumEB),2) [Surcharge],b.arrearctr " +
    "		FROM " +
    "		( " +
    "			SELECT arstrxhdr.consumerid,sum(arstrxhdr.trxbalance) as [SumEB],sum(arstrxhdr.vatbalance) [SumVAT], " +
    "			cons.consumertypeid,typ.scharge1 [scmult],arsbillperiod.seq,arsbillperiod.billperiod, cons.[name], cons.[address] " +
    "			FROM arstrxhdr LEFT OUTER JOIN arsbillperiod " +
    "			ON arstrxhdr.billperiod = arsbillperiod.billperiod " +
    "			LEFT JOIN arsconsumer cons " +
    "			on arstrxhdr.consumerid=cons.consumerid " +
    "			LEFT JOIN arstype typ " +
    "			ON cons.consumertypeid=typ.consumertypeid " +
    "			WHERE (trxbalance > 0 or vatbalance > 0) " +
    "			AND substring(arstrxhdr.reference,1,1)<>'P' " +
    "			AND arstrxhdr.trxid<>'CM' " +
    "			AND arstrxhdr.trxamount > 0 " +
    "			AND arstrxhdr.consumerid in (select accountno from tblEmpCareOff) " +
    "			GROUP BY arstrxhdr.consumerid,cons.name,cons.consumertypeid,typ.scharge1,arsbillperiod.seq,arsbillperiod.billperiod, cons.[address] " +
    "		) a inner join " +
    "		( " +
    "			SELECT arstrxhdr.consumerid,COUNT(arsbillperiod.seq) arrearctr " +
    "			FROM arstrxhdr LEFT OUTER JOIN arsbillperiod " +
    "			ON arstrxhdr.billperiod = arsbillperiod.billperiod " +
    "			WHERE (trxbalance > 0 or vatbalance > 0) " +
    "			AND substring(arstrxhdr.reference,1,1)<>'P' " +
    "			AND arstrxhdr.trxid<>'CM' " +
    "			AND arstrxhdr.trxamount > 0 " +
    "			AND arstrxhdr.consumerid in (select accountno from tblEmpCareOff) " +
    "			GROUP BY arstrxhdr.consumerid " +
    "		) b " +
    "		on a.consumerid=b.consumerid " +
    "		left join " +
    "		( " +
    "			SELECT consumerid,case when CAST(aa.[Due Date] AS DATE)<CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END [ADDEND] " +
    "			FROM " +
    "			( " +
    "				SELECT arstrxhdr.consumerid,CONVERT(varchar(10), " +
    "				CASE WHEN datepart(dw,dateadd(day,10,MIN(arstrxhdr.trxdate)))=7 THEN  " +
    "					dateadd(day,12,MIN(arstrxhdr.trxdate)) " +
    "				ELSE " +
    "					CASE WHEN datepart(dw,dateadd(day,10,MIN(arstrxhdr.trxdate)))=1 THEN " +
    "						dateadd(day,11,MIN(arstrxhdr.trxdate)) " +
    "					ELSE  " +
    "						dateadd(day,10,MIN(arstrxhdr.trxdate)) " +
    "					END " +
    "				END, " +
    "				101) [Due Date] " +
    "				FROM arstrxhdr LEFT OUTER JOIN arsbillperiod " +
    "				ON arstrxhdr.billperiod = arsbillperiod.billperiod " +
    "				WHERE (trxbalance > 0 or vatbalance > 0) " +
    "				AND substring(arstrxhdr.reference,1,1)<>'P' " +
    "				AND arstrxhdr.trxid<>'CM' " +
    "				AND arstrxhdr.trxamount > 0 " +
    "				AND arstrxhdr.consumerid in (select accountno from tblEmpCareOff) " +
    "				AND arsbillperiod.seq = 0 " +
    "				Group By arstrxhdr.consumerid " +
    "			) aa " +
    "		) c " +
    "		on a.consumerid=c.consumerid " +
    "	)abc " +
    "	group by consumerid,[name],[address], arrearctr " +
    ") src " +
    "on tblEmpCareOff.accountno=src.[ACCOUNT NO] " +
    "inner join arsconsumer " +
    "on tblEmpCareOff.accountno=arsconsumer.consumerid " +
    "order by tblEmp.id;";
    }
}