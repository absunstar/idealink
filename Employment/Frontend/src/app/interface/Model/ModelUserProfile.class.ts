import { UserType } from 'src/app/Enum/UserType.enum';

export class ModelUserProfile{
    Id: string;
    Name: string;
    Email:string;
    Type:UserType;
    password:string;
    SelectedPartnerEntityId: string[];
    SelectedSubPartnerEntityId: string[];
}