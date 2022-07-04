import { UserProfile } from 'src/app/model/user-profile';
import { cTraineeItem } from './Trainee.class';

export class MyTraining{
    Profile: cTraineeItem;
    trainings: MyTrainingItems[];
}
export class MyTrainingItems{
    TrainingId:string;
    Name: string;
    Date:string;
    HasCertificate: boolean;
    ExamCount:number;
    CertificatePath:string;
}