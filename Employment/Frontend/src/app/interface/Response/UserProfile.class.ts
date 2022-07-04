import { UserType } from 'src/app/Enum/UserType.enum';

export class cUserProfileList{
    totalCount: number;
    lstResult:cUserProfileItem[] ;
    pageSize: number;
}
export class cUserProfileItem{
    Id: string;
    Name: string;
    IsActive: boolean;
    Email:string;
    Type:UserType;
    IsEmployerLimitedCompanies:boolean;
    MyCompanies: cEntityObj[];
}

export class cEntityObj{
    _id: string;
    Name: string;
    IsApproved:boolean;
}