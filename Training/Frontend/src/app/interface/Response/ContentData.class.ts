import { EnumContentData } from 'src/app/Enum/ContentData.enum';

export class cContentDataList{
    totalCount: number;
    lstResult:cContentDataItem[] ;
    pageSize: number;
}
export class cContentDataItem{
    _id: string;
    Name: string;
    IsActive: boolean;
    Type:EnumContentData;
    Data:string;
}