export class ModelJobSeeker {
    _id: string;
    Name: string;
    UserId: string;
    JobTitle: string;
    DOB: Date;
    Gender:number;
    Email: string;
    Phone: string;
    Website: string;
    ExperienceId: string;
    QualificationId: string;
    About: string;
    SocialFacebook: string;
    SocialTwitter: string;
    SocialLinkedin: string;
    SocialGooglePlus: string;
    Languages: string[];
    CountryId: string;
    CityId: string;
    CoverLetterFile: string;
    ResumeFile: string;
    ProfilePicture:string
}
export class ModelResumeItem {
    _id: string;
    Name: string;
    SubTitle: string;
    StartDate: Date;
    EndDate: Date;
    Description: string;
}
export class ModelResumeCertification {
    _id: string;
    Name: string;
    StartDate: Date;
    Description: string;
    CertificatePath:string;
}