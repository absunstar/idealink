export class ModelReportDates{
    StartDate :Date;
    EndDate:Date;
}
export class ModelReportJob extends ModelReportDates{
    CompanyId :string;
    JobFieldId:string;
}