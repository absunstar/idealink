import { UserType } from 'src/app/Enum/UserType.enum';

export class ModelUserProfile {
    Id: string;
    Name: string;
    Email: string;
    Type: UserType;
    CityId: string;
    AreaId:string;
    TrainerTrainingDetails: string;
    TrainerStartDate: Date;
    TrainerEndDate: Date;
    SelectedPartnerEntityId: string[];
    SelectedSubPartnerEntityId: string[];
}