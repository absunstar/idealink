
export interface cGenericIdNameList {
    totalCount: number;
    lstResult: cGenericIdNameItem[];
    pageSize: number;
}
export class cGenericIdNameItem {
    _id: string;
    Name: string;
    IsActive: boolean;
    subItems: cGenericSubItem[];
    Count:number;
}
export class cGenericSubItem {
    constructor() {
        this.Name = "";
        this._id = "";
    }
    _id: string;
    Name: string;
    IsActive: boolean;
    Count:number;
}

export class cGenericSubItemURL extends cGenericSubItem {
    URL: string;
}