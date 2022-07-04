import { ModelDynamic } from '../Model/ModelDynamic.class';

export interface cJobFairList {
    totalCount: number;
    lstResult: cJobFairItem[];
    pageSize: number;
}
export class cJobFairItem{
    Name: string;
    _id: string;
    IsActive: boolean;
    EventDate: Date;
    Location: string;
    IsOnline: boolean;
    ShortDescription: string;
    Field:string;
    data: ModelDynamic[];
}