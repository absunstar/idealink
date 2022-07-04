import { ICourse } from './Course.interface';
import { cTrainingTypeItem } from './TrainingType.class';

export interface ITrainingCategory{
    totalCount: number;
    lstResult:ITrainingCategoryItem[] ;
    pageSize: number;
}
export class ITrainingCategoryItem{
    Id: string;
    Name: string;
    IsActive: boolean; 
    TrainingType: cTrainingTypeItem;
    Course: ICourse[];
}