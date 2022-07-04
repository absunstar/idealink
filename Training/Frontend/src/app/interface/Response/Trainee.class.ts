import { ModelDynamic } from '../Model/ModelDynamic.class';

export class cTraineeList{
    totalCount: number;
    lstResult:cTraineeItem[] ;
    pageSize: number;
}
export class cTraineeItem{
    
    Id: string;
    Name: string;
    IsActive: boolean;
    Email:string;
    Mobile:string;
    NationalId:string;
    Gender:number =1;
    IdType:number = 1; //NationalID
    DOB:Date;
    data:ModelDynamic[];
}
export class cTraineeItemInfo extends cTraineeItem{
    IsApproved:boolean;
}