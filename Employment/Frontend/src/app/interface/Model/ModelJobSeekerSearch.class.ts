import { ModelPaging } from './ModelPaging.interface';

export class ModelJobSeekerSearch extends ModelPaging
{
    ExperienceId: string[];
    GenderId: string[];
    Qualificationid: string[];
    LanguageId:string[];
    CountryId:string;
    CityId:string;
    filterText:string;
}