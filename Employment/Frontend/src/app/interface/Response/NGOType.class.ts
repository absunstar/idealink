
export interface cNGOTypeList{
    totalCount: number;
    lstResult:cNGOTypeItem[] ;
    pageSize: number;
}
export class cNGOTypeItem{
    Id: string;
    Name: string;
    IsActive: boolean;
}