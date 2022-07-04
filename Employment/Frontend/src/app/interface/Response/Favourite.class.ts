
export interface cFavouriteList{
    totalCount: number;
    lstResult:cFavouriteItem[] ;
    pageSize: number;
}
export class cFavouriteItem{
    _id: string;
    Name: string;
    Title:string;
    EntityId:string;
    CreatedAd:Date;
    ImageURL:string;
    ResumeURL:string;
}
