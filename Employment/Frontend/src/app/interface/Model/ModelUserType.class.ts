export class UserTypeList{

    private lst: UserTypeItem[] = [];

    constructor()
    {
        this.lst.push(new UserTypeItem(1,"Admin"));
        this.lst.push(new UserTypeItem(2,"Employer"));
        this.lst.push(new UserTypeItem(3,"Job Seeker"));
    }
    getUserTypes():UserTypeItem[]{
        return this.lst.filter(x=>x.Id >= 1);
    }
}
export class UserTypeItem{
    constructor(id:number, name:string)
    {
        this.Id = id;
        this.Name = name;
    }
    Id: number;
    Name: string;
   
}