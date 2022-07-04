
export interface cTrainingTypeList{
    totalCount: number;
    lstResult:cTrainingTypeItem[] ;
    pageSize: number;
}
export class cTrainingTypeItem{
    Id: string;
    Name: string;
    IsActive: boolean;
}