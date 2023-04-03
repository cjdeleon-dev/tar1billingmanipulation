select asl.consumerid,cons.name,asl.reason,asl.dtdread,asl.userid,asl.actdate 
from arsstatuslog asl inner join arsconsumer cons
on asl.consumerid=cons.consumerid
where asl.actdate = '2023-01-27' and asl.newstatusid='D' and asl.consumerid like '__0807%' and isnull(asl.isdtdsumrpt,0)=0





select * from secuser where userid='1401'