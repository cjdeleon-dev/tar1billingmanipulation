using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAR1ORDATA.Queries
{
    public class ForWriteOffQueries
    {
        public static readonly string sqlGetAllForWriteOff = "select arstrxhdr.consumerid, arsconsumer.name, arsconsumer.address,arsconsumer.consumertypeid, arsconsumer.statusid, sum(arstrxhdr.trxbalance) [balance] " +
                                                            "from arstrxhdr inner join arsconsumer " +
                                                            "on arstrxhdr.consumerid=arsconsumer.consumerid " +
                                                            "where trxid='EB' and arsconsumer.statusid='D' and arsconsumer.consumertypeid in ('R','C','I') and arsconsumer.address like '%' + @area + '%' " +
                                                            "group by arstrxhdr.consumerid, arsconsumer.name,arsconsumer.address,arsconsumer.statusid,arsconsumer.consumertypeid " +
                                                            "having max(CAST(arstrxhdr.billperiod AS INT))<=CAST(CAST(@lastbillperiod as VARCHAR(4)) + '12' as INT) " + 
                                                            "and sum(arstrxhdr.trxbalance)>0;";

        public static readonly string sqlGetYearOf = "select * from (VALUES(1, 2012),(2, 2013),(3, 2014),(4, 2015),(5, 2016),(6, 2017),(7, 2018),(8, 2019),(9, 2020),(10, 2021),(11, 2022)) q(id,yearof);";
    }
}