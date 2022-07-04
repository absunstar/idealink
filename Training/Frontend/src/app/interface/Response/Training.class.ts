import { cTraineeItem, cTraineeItemInfo } from './Trainee.class';

export class cTrainingList {
    totalCount: number;
    lstResult: cTrainingItem[];
    pageSize: number;
}
export class cTrainingItem {
    Id: string;
    PartnerId: ItemDetails;
    SubPartnerId: ItemDetails;
    TrainingCenterId: ItemDetails;
    TrainerId: string;
    TrainerDetails: ItemDetails;
    TrainerCount:number;
    TrainingTypeId: ItemDetails;
    TrainingCategoryId: ItemDetails;
    CityId: string;
    AreaId: string;
    StartDate: Date;
    EndDate: Date;
    days: string[];
    CanEdit: boolean;
    IsAdminApproved: boolean;
    IsConfirm1: boolean;
    IsConfirm2: boolean;
    Trainees: cTraineeItemInfo[];
    Sessions: cSessionItem[];
    Attendances:cAttendance[];
    Type:number;
    ExamTemplateId:string;
    IsOnline:boolean;
}
export class ItemDetails {
    Id: string;
    Name: string;
}
export class cSessionItem {
    Id: string;
    Name: string;
    Day: Date;
    IsAttendanceFilled: boolean;
}
export class cAttendance
{
    SessionId:string;
    Attendances:cAttendanceTrainee[];
}
export class cAttendanceTrainee
{
    TraineeId:string;
    IsAttendant:boolean;
}