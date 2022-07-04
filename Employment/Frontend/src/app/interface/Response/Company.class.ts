import { ModelDynamic } from "../Model/ModelDynamic.class";

export interface cCompanyList{
    totalCount: number;
    lstResult:cCompanyItem[] ;
    pageSize: number;
}
export class cCompanyItem{
    _id: string;
    Name: string;
    IsActive: boolean;
    Email:string;
    Phone:string;
    Website:string;
    Establish:Date;
    Industry:cSubItem;
    About:string;
    SocialFacebook:string;
    SocialTwitter:string;
    SocialLinkedin:string;
    SocialGooglePlus:string;
    Country:cSubItem;
    City:cSubItem;
    Address:string;
    CompanyLogo:string;
    IsApproved :boolean;
    data:ModelDynamic[];
}
export class cSubItem{
    _id: string;
    Name: string;
}
