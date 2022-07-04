

export interface cApplyList{
    totalCount: number;
    lstResult:cApplyItem[] ;
    pageSize: number;
}
export class cApplyItem{
    _id: string;
    Message: string;
    CraetedAt: Date;
    Job:ApplySubItem;
    JobSeeker:ApplySubItem;
    IsHired:boolean;
}
export class ApplySubItem
{
     _id : string;
     Name : string;
     SubName : string;
     URL:string;
}
export class ReportApply{
    JobName:string;
    JobId:string;
    Count: number;
}