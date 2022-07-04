import { JobStatus } from 'src/app/Enum/JobStatus.enum';
import { cGenericSubItem, cGenericSubItemURL } from './GenericIdName.class';
import { JobType } from 'src/app/Enum/JobType.enum';

export interface cJobList {
    totalCount: number;
    lstResult: cJobItem[];
    pageSize: number;
}
export class cJobItem {
    _id: string;
    Name: string;
    IsActive: boolean;
    Gender: number
    Company: cGenericSubItemURL;
    Description: string;
    Skills:string;
    Benefits:string;
    Status: JobStatus;
    Type: JobType;
    Deadline: Date;
    JobField: cGenericSubItem;
    JobSubField: cGenericSubItem;
    Experience: cGenericSubItem;
    Industry: cGenericSubItem;
    Qualification: cGenericSubItem;
    Country: cGenericSubItem;
    City: cGenericSubItem;
    Address: string;
    CreatedAt: Date;
    ApplicantCount:number;
    Remuneration:string;
}
export class ReportJobCount{
    Company: cGenericSubItemURL;
    Count:number;
}