import { ModelPaging } from './ModelPaging.interface';

export class ModelJobSearch extends ModelPaging
{
    ExperienceId: string[];
    GenderId: string[];
    Qualificationid: string[];
    IndustryId:string[];
    JobFieldId:string[];
    CountryId:string;
    CityId:string;
}