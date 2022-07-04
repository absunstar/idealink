import { ModelDynamic } from './ModelDynamic.class';

export class ModelJobFair{
    Name: string;
    _id: string;
    EventDate: Date;
    Location: string;
    IsOnline: boolean;
    ShortDescription: string;
    Field:string;
    data:ModelDynamic[];
}

