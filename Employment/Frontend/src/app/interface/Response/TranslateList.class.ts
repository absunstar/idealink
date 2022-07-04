import { TranslateType } from 'src/app/Enum/TranslateType.enum';

export class TranslateList{
    Type:TranslateType;
    Data:TranslateListData[];
}
export class TranslateListData{
    _id :string;
    Name:string;
    Name2:string;
}