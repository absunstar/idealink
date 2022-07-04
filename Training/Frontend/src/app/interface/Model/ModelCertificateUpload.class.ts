import { CertificateType } from 'src/app/Enum/CertificateType.enum';

export class ModelCertificateUpload{
    Type: CertificateType;
    partnerId:string;
    TrainingCenterId:string;
    TrainingTypeId:string;
    TrainingCategoryId:string;
}