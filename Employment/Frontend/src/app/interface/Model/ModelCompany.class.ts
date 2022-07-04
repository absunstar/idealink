import { ModelDynamic } from "./ModelDynamic.class";

export class ModelCompany{
    constructor(){
        this.IndustryId = "-1";
        this.CityId = "-1";
        this.CountryId = "-1";
    }
    _id:string;
    Name:string;
    Email:string;
    Phone:string;
    Website:string;
    Establish:Date;
    IndustryId:string = "-1";
    About:string;
    SocialFacebook:string;
    SocialTwitter:string;
    SocialLinkedin:string;
    SocialGooglePlus:string;
    CountryId:string = "-1";
    CityId:string = "-1";
    Address:string;
    CompanyLogo:string;
    data:ModelDynamic[];
}