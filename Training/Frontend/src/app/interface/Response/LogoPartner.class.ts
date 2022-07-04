
export class cLogoPartnerList{
    totalCount: number;
    lstResult:cLogoPartnerItem[] ;
    pageSize: number;
}
export class cLogoPartnerItem{
    _id: string;
    IsActive: boolean;
    WebsiteURL:string;
    ImagePath:string;
}