import { cEntityPartnerItem } from './EntityPartner.class';

export class cEntityTrainingCenterList{
    totalCount: number;
    lstResult:cEntityTrainingCenterItem[] ;
    pageSize: number;
}
export class cEntityTrainingCenterItem{
    Id: string;
    Name: string;
    IsActive: boolean;
    Phone:string;
}