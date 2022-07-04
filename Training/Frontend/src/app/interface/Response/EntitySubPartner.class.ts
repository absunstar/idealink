import { cEntityPartnerItem } from './EntityPartner.class';
import { cEntityTrainingCenterItem } from './EntityTrainingCenter.class';
import { cUserProfileItem } from './UserProfile.class';

export class cEntitySubPartnerList{
    totalCount: number;
    lstResult:cEntitySubPartnerItem[] ;
    pageSize: number;
}
export class cEntitySubPartnerItem{
    Id: string;
    Name: string;
    IsActive: boolean;
    Phone:string;
    Partners:cEntityPartnerItem[];
    TrainingCenters:cEntityTrainingCenterItem[];
    MemberSubPartners: cUserProfileItem[];
}