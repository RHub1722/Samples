select Project.ProjectID ,
 Project.Description,
 Vendor.VendorID,
 Vendor.Name,
 JobHead.JobNum,
 JobHead.PartNum,
 JobOper.OprSeq,
 JobOper.QtyPer,
 SBExecDate.Calculated_LaborQty,
 SBExecDate.Calculated_ExecutionDate,
 Dic1.UD40_Character01,
 JobOper.Comment_c,
 Dic2.UD401_Character01,
 JobOper.ExecutionComment_c,
 JobOper.OpComplete,
 (EmpBasic.FirstName + ' ' + EmpBasic.LastName) as Calculated_ProjectOwner
from Erp.JobHead as JobHead
left outer join Erp.Project as Project on  JobHead.Company = Project.Company And JobHead.ProjectID = Project.ProjectID

inner join Erp.JobOper as JobOper on  JobHead.Company = JobOper.Company And JobHead.JobNum = JobOper.JobNum and JobOper.SubContract = 1

inner join Erp.Vendor as Vendor on  JobOper.Company = Vendor.Company And JobOper.VendorNum = Vendor.VendorNum

left outer join  (select 
 UD40.Company as UD40_Company,
 (CONVERT(INT, Key3)) as Calculated_Key,
 UD40.Character01 as UD40_Character01
from Ice.UD40 as UD40
 where (UD40.Key1 = 'JobOper'  and UD40.Key2 = 'CurrentStatus'))  as Dic1 on JobOper.CurrentStatus_c = Dic1.Calculated_Key
And JobOper.Company = Dic1.UD40_Company

left outer join  (select 
 UD401.Company as UD401_Company,
 UD401.Key3 as UD401_Key3,
 UD401.Character01 as UD401_Character01
from Ice.UD401 as UD401
 where (UD401.Key1 = 'JobOper'  and UD401.Key2 = 'ExecutionStatus'))  as Dic2 on  JobOper.ExecutionStatus_c = Dic2.UD401_Key3
And JobOper.Company = Dic2.UD401_Company

--start SubQury
left outer join  (select 
 JobOper1.Company as JobOper1_Company,
 JobOper1.JobNum as JobOper1_JobNum,
 JobOper1.OprSeq as JobOper1_OprSeq,
 (case
     when SubShipH.ShipDate != '' then SubShipH.ShipDate
     else RcvDtl.ReceiptDate
 end) as Calculated_ExecutionDate,
 (case
  when SubShipD.ShipQty > 0 then SubShipD.ShipQty
  else RcvDtl.OurQty
 end) as Calculated_LaborQty

from Erp.JobOper as JobOper1
left outer join Erp.SubShipD as SubShipD on JobOper1.Company = SubShipD.Company AND JobOper1.JobNum = SubShipD.JobNum
And JobOper1.AssemblySeq = SubShipD.AssemblySeq And JobOper1.OprSeq = SubShipD.OprSeq

left outer join Erp.SubShipH as SubShipH on SubShipD.Company = SubShipH.Company
And SubShipD.PackNum = SubShipH.PackNum

left outer join Erp.RcvDtl as RcvDtl on JobOper1.Company = RcvDtl.Company And JobOper1.JobNum = RcvDtl.JobNum
And JobOper1.AssemblySeq = RcvDtl.AssemblySeq And JobOper1.OprSeq = RcvDtl.JobSeq)  as SBExecDate on JobOper.Company = SBExecDate.JobOper1_Company
And JobOper.JobNum = SBExecDate.JobOper1_JobNum AND JobOper.OprSeq = SBExecDate.JobOper1_OprSeq
--end SubQury

inner join Erp.UserComp as UserComp on Project.Company = UserComp.Company And Project.ProjectOwner_c = UserComp.DcdUserID
inner join Erp.EmpBasic as EmpBasic on UserComp.Company = EmpBasic.Company And UserComp.EmpID = EmpBasic.EmpID





