import { cArea } from './Area.class';

export class cCityList{
    totalCount: number;
    lstResult:cCityItem[] ;
    pageSize: number;
}
export class cCityItem{
    Id: string;
    Name: string;
    IsActive: boolean;
    Areas: cArea[];
}