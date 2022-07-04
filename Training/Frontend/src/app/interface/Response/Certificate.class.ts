import { CertificateType } from 'src/app/Enum/CertificateType.enum';

export class cCertificateList{
    totalCount: number;
    lstResult:cCertificateItem[] ;
    pageSize: number;
}
export class cCertificateItem{
    Type: CertificateType;
    PartnerName: string;
    TrainingCenterName: string;
    TrainingCategoryName: string;
    TrainingTypeName:string;
    FileName:string;
    IsActive:boolean;
    _id:string;
}