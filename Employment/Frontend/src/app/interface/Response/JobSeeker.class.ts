import { cGenericSubItem } from './GenericIdName.class';

export interface cJobSeekerList {
    totalCount: number;
    lstResult: cJobSeekerItem[];
    pageSize: number;
}
export class cJobSeekerItem {
    _id: string;
    Name: string;
    IsMyResume: boolean;
    IsActive: boolean;
    JobTitle: string;
    DOB: Date;
    Gender: number;
    Email: string;
    Phone: string;
    Website: string;
    Experience: cGenericSubItem;
    Qualification: cGenericSubItem;
    About: string;
    SocialFacebook: string;
    SocialTwitter: string;
    SocialLinkedin: string;
    SocialGooglePlus: string;
    Languages: cGenericSubItem[];
    Country: cGenericSubItem;
    City: cGenericSubItem;
    Education: ResponseResumeItem[];
    WorkHistory: ResponseResumeItem[];
    ExtraCurricular: ResponseResumeItem[];
    Certification: ResponseResumeCertification[];
    CoverLetterFile: string;
    ResumeFile: string;
    ProfilePicture: string;
    ContactPermissionHasPermission:number;
}
export class ResponseResumeItem 
{
    _id: string;
    Name: string;
    IsActive: boolean;
    SubTitle: string;
    StartDate: Date;
    EndDate: Date;
    Description: string;
}
export class ResponseResumeCertification 
{
    _id: string;
    Name: string;
    IsActive: boolean;
    StartDate: Date;
    Description: string;
    CertificatePath:string;
}
export class ResponseContactInformationRequest{
    EmployerId:string;
    CompanyId:string;
    CompanyName:string;
    IsApproved:number;
    CreatedAt:Date;
}
export interface cResponseContactInformationRequestList {
    totalCount: number;
    lstResult: ResponseContactInformationRequest[];
    pageSize: number;
}