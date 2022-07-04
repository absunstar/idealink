import { UserType } from 'src/app/Enum/UserType.enum';
import { UserProfile } from 'src/app/model/user-profile';

export class cUserProfileList {
    totalCount: number;
    lstResult: cUserProfileItem[];
    pageSize: number;
}
export class cUserProfileItem {
    Id: string;
    Name: string;
    IsActive: boolean;
    Email: string;
    Type: string;
    CityId: string;
    AreaId: string;
    TrainerTrainingDetails: string;
    TrainerStartDate: Date;
    TrainerEndDate: Date;
    MyPartnerListIds: cEntityObj[];
    MySubPartnerListIds: cEntityObj[];
}

export class cEntityObj {
    Id: string;
    Name: string;
}

export class cUserProfileTrainerCertificateList {
    totalCount: number;
    lstResult: cUserProfileTrainerCertificateItem[];
    pageSize: number;
}


export class cUserProfileTrainerCertificateItem {
    TrainerId: string;
    TrainerName: string;
    PartnerId: string;
    PartnerName: string;
    TrainingCategoryId: string;
    TrainingCategoryName: string;
    TrainingTypeId: string;
    TrainingTypeName: string;
    ExamCount: number;
    CertificatePath:string;
}
export class cUserProfileTrainerCertificateItemWithProfile {
    TrainerName: string;
    lstResult: cUserProfileTrainerCertificateItem[];
}