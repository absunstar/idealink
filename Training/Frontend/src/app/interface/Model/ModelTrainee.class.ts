import { ModelDynamic } from './ModelDynamic.class';

export class ModelTrainee{
    Id: string;
    Name:string;
    Email:string;
    Mobile:string;
    NationalId:string;
    Gender:string="1";
    IdType:string = "1"; //NationalID
    DOB:Date;
    TrainingId:string;
    data:ModelDynamic[];
}