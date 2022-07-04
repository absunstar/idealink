
export interface cExamTemplateList{
    totalCount: number;
    lstResult:cExamTemplateItem[] ;
    pageSize: number;
}
export class cExamTemplateItem{
    _id: string;
    Name:string;
    IsActive: boolean;
    Easy: number;
    Medium: number;
    Hard:number;
}