import { NumberFixedLenPipe } from "ng-pick-datetime/date-time/numberedFixLen.pipe";
import { ModelPaging } from "./ModelPaging.interface"

export class ModelAdminJobSearch extends ModelPaging{
    CompanyId:string;
    StatusId: number;
}