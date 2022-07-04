import { cUserProfileItem } from './UserProfile.class';
import { cEntityTrainingCenterItem } from './EntityTrainingCenter.class';

export class cEntityPartnerList{
    totalCount: number;
    lstResult:cEntityPartnerItem[] ;
    pageSize: number;
}
export class cEntityPartnerItem{
    Id: string;
    Name: string;
    IsActive: boolean;
    Phone:string;
    MinHours:number;
    MaxHours:number;
    Members : cUserProfileItem[];
    TrainingCenters: cEntityTrainingCenterItem[];

}