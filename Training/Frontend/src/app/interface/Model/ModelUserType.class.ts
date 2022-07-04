import { TranslateService } from '@ngx-translate/core';

export class UserTypeList {

    private lst: UserTypeItem[] = [];
    strAdmin: string;
    strPartner: string;
    strSubPartner: string;
    strTrainer: string;

    constructor(protected BLTranslate: TranslateService) {


        this.BLTranslate.get("GenericAdmin").subscribe(res => { this.strAdmin = res; });
        this.BLTranslate.get("GenericPartner").subscribe(res => { this.strPartner = res; });
        this.BLTranslate.get("GenericSubPartner").subscribe(res => { this.strSubPartner = res; });
        this.BLTranslate.get("GenericTrainer").subscribe(res => { this.strTrainer = res; });

        this.lst.push(new UserTypeItem(1, this.strAdmin));
        this.lst.push(new UserTypeItem(2, this.strPartner));
        this.lst.push(new UserTypeItem(3, this.strSubPartner));
        this.lst.push(new UserTypeItem(4, this.strTrainer));
        //dthis.lst.push(new UserTypeItem(5,"Trainee"));
    }
    getAdminUserTypes(): UserTypeItem[] {
        return this.lst.filter(x => x.Id >= 1);
    }
    getPartnerUserTypes(): UserTypeItem[] {
        return this.lst.filter(x => x.Id > 2);
    }
    getSubPartnerUserTypes(): UserTypeItem[] {
        return this.lst.filter(x => x.Id > 3);
    }
    getUserListByType(type: string): UserTypeItem[] {
        switch (type.toLowerCase()) {
            case "admin": return this.getAdminUserTypes();
            case "partner": return this.getPartnerUserTypes();
            case "subpartner": return this.getSubPartnerUserTypes();
            default: return new UserTypeItem[0];
        }
    }
}
export class UserTypeItem {
    constructor(id: number, name: string) {
        this.Id = id;
        this.Name = name;
    }
    Id: number;
    Name: string;
}