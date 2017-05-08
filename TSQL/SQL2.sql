select 
 JobHead.ProjectID as JobHead_ProjectID,
 Project.Description as Project_Description,
 JobHead.JobNum as JobHead_JobNum,
 JobHead.SchedCode as JobHead_SchedCode,
 JobHead.JobType as JobHead_JobType,
 JobHead.PartDescription as JobHead_PartDescription,
 JobHead.ReqDueDate as JobHead_ReqDueDate,
 JobOper.OpDesc as JobOper_OpDesc,
 JobOper.QtyPer as JobOper_QtyPer,
 JobOper.CurrentStatus_c as JobOper_CurrentStatus_c,
 JobOpDtl.ResourceID as JobOpDtl_ResourceID,
 JobOper.ExecutionStatus_c as JobOper_ExecutionStatus_c,
 JobOper.ExecutionComment_c as JobOper_ExecutionComment_c,
 JobOper.StarDate_c as JobOper_StarDate_c,
 JobOper.EndDate_c as JobOper_EndDate_c,
 JobOper.AssemblySeq as JobOper_AssemblySeq,
 JobHead.JobReleased as JobHead_JobReleased,
 JobHead.JobEngineered as JobHead_JobEngineered,
 JobAsmbl.JobNum as JobAsmbl_JobNum,
 JobAsmbl.AssemblySeq as JobAsmbl_AssemblySeq,
 JobOpDtl.JobNum as JobOpDtl_JobNum,
 JobOpDtl.AssemblySeq as JobOpDtl_AssemblySeq,
 JobOpDtl.OprSeq as JobOpDtl_OprSeq,
 JobOpDtl.OpDtlSeq as JobOpDtl_OpDtlSeq,
 JobOper.OprSeq as JobOper_OprSeq,
 JobOper.JobNum as JobOper_JobNum
from Erp.JobOpDtl as JobOpDtl
inner join Erp.JobOper as JobOper on JobOpDtl.Company = JobOper.Company And JobOpDtl.JobNum = JobOper.JobNum
And JobOpDtl.AssemblySeq = JobOper.AssemblySeq And  JobOpDtl.OprSeq = JobOper.OprSeq
 And JobOper.OpComplete = 0  And JobOper.SubContract = 0  And JobOper.ExecutionStatus_c <> '4' 

inner join Erp.JobHead as JobHead on JobOper.Company = JobHead.Company And JobOper.JobNum = JobHead.JobNum
 And JobHead.JobClosed = 0  and JobHead.JobComplete = 0  And JobHead.JobReleased = 1  And JobHead.JobEngineered = 1  
  And (JobHead.JobType = 'MFG'  or JobHead.JobType = 'MNT' ) And JobHead.InCopyList = 0

inner join Erp.JobAsmbl as JobAsmbl on  JobHead.Company = JobAsmbl.Company And JobHead.JobNum = JobAsmbl.JobNum

left outer join Erp.Project as Project on JobHead.Company = Project.Company
And JobHead.ProjectID = Project.ProjectID

left outer join  (select 
 LaborDtl.JobNum as LaborDtl_JobNum,
 LaborDtl.AssemblySeq as LaborDtl_AssemblySeq,
 LaborDtl.OprSeq as LaborDtl_OprSeq,
 LaborDtl.Company as LaborDtl_Company
from Erp.LaborDtl as LaborDtl
  where LaborDtl.OpComplete = 0
group by LaborDtl.JobNum,
 LaborDtl.AssemblySeq,
 LaborDtl.OprSeq,
 LaborDtl.Company) as SubQuery2 on JobOper.Company = SubQuery2.LaborDtl_Company
And JobOper.JobNum = SubQuery2.LaborDtl_JobNum
And JobOper.AssemblySeq = SubQuery2.LaborDtl_AssemblySeq
And JobOper.OprSeq = SubQuery2.LaborDtl_OprSeq
 where JobOpDtl.ResourceGrpID = 'GCONS'