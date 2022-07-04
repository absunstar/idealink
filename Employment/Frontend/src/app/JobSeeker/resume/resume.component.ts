import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cJobSeekerItem, ResponseResumeItem, ResponseResumeCertification } from 'src/app/interface/Response/JobSeeker.class';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ActivatedRoute } from '@angular/router';
import { ServiceJobSeeker } from 'src/app/services/job-seeker.service';
import { cGenericSubItem, cGenericIdNameItem } from 'src/app/interface/Response/GenericIdName.class';
import { retry, takeWhile } from 'rxjs/operators';
import { ModelJobSeeker, ModelResumeCertification } from 'src/app/interface/Model/ModelJobSeeker.class';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ServiceQualification } from 'src/app/services/qualification.service';
import { ServiceYearsOfExperience } from 'src/app/services/years-of-experience.service';
import { ServiceLanguages } from 'src/app/services/languages.service';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ServiceCountry } from 'src/app/services/country.service';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { ConfirmationDialogService } from 'src/app/common/confirmation-dialog/confirmation-dialog.service';
import { ServiceFiles } from 'src/app/services/files.service';
import { TranslateService } from '@ngx-translate/core';
import { ModelId } from 'src/app/interface/Model/ModelId.interface';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-resume',
  templateUrl: './resume.component.html',
  styleUrls: ['./resume.component.css'],
})
export class ResumeComponent extends baseComponent implements OnInit {

  JobSeekerId: string = "";
  objJobSeeker: cJobSeekerItem = new cJobSeekerItem();
  modelJobSeeker: ModelJobSeeker = new ModelJobSeeker();
  IsEditDescription: boolean = false;
  IsEditProfile: boolean = false;
  IsDatesCorrect: boolean = true;

  LkupQualification: cGenericSubItem[];
  LkupExperience: cGenericSubItem[];
  LkupLanguage: cGenericSubItem[];
  LkupCountry: cGenericIdNameItem[];
  LkupCity: cGenericSubItem[];

  modelDOB: NgbDateStruct;
  modelEduStart: NgbDateStruct;
  modelEduEnd: NgbDateStruct;
  dateOld: NgbDateStruct;

  modelIsCreate: boolean;
  modelTitle: string;
  modelNameEdited: string;
  modelEducation: ResponseResumeItem;
  modelIsEducation: boolean;
  modelIsWork: boolean;
  modelIsExtra: boolean;
  modelCertificate: ModelResumeCertification;
  IsFavourite: boolean;
  FileToUpload: File;
  SeekerCertifiateFileError: string;
  isClicked:boolean = false;

  constructor(
    private BLServiceFile: ServiceFiles,
    private BLJobSeeker: ServiceJobSeeker,
    private confirmationDialogService: ConfirmationDialogService,
    BLServiceShowMessage: ServiceShowMessage,
    BLJobSeekerLoginUser: ServiceLoginUser, BLTranslate: TranslateService,
    private BLQualification: ServiceQualification,
    private BLExperience: ServiceYearsOfExperience,
    private BLFavourite: ServiceFavourite,
    private BLCountry: ServiceCountry,
    private BLLanguages: ServiceLanguages,
    private modalService: NgbModal,
    private cookies :CookieService,
    private route: ActivatedRoute) {
    super(BLServiceShowMessage, BLJobSeekerLoginUser, BLTranslate)
    this.BLTranslate.get("SeekerCertifiateFileError").subscribe(res => { this.SeekerCertifiateFileError = res; });
  }
  getData() {
    this.loadData();
    this.LoadFav();
    this.BLCountry.getListActive().subscribe({
      next: lst => {
        this.LkupCountry = lst;
        this.onCountrySelect();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLQualification.getListActive().subscribe({
      next: lst => {
        this.LkupQualification = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLExperience.getListActive().subscribe({
      next: lst => {
        this.LkupExperience = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.BLLanguages.getListActive().subscribe({
      next: lst => {
        this.LkupLanguage = lst;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });

    var d = new Date();
    this.dateOld = {
      year: d.getUTCFullYear() - 100, month: d.getUTCMonth() + 1
      , day: d.getUTCDate()
    };
  }
  ngOnInit(): void {

    this.cookies.set('priST',JSON.stringify(true))
    const param = this.route.snapshot.paramMap.get('Id');
    if (param)
      this.JobSeekerId = param;
    if (this.IsLoggedIn) {
      this.getData();
    }
    else {
      this.BLServiceLoginUser.UserIsLoggedInChanged.pipe(takeWhile(val => true)).subscribe(obj => {
        if (obj) {
          this.getData();

        }
      });
    }

  }
  onCountrySelect() {
    this.LkupCity = [];

    var sub = this.LkupCountry?.find(x => x._id == this.modelJobSeeker.CountryId)?.subItems;
    if (sub?.length == 0)
      return;

    this.LkupCity = sub?.filter(y => y.IsActive == true);;
  }
  LoadFav() {
    this.BLFavourite.CheckMyFavourite(this.JobSeekerId).subscribe({
      next: obj => {
        this.IsFavourite = obj;
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  loadData() {
    this.BLJobSeeker.getGetByid(this.JobSeekerId).subscribe({
      next: obj => {
        this.objJobSeeker = obj;
        this.objJobSeeker.ProfilePicture != "" ? this.objJobSeeker.ProfilePicture = this.objJobSeeker.ProfilePicture + "?" + (new Date()).getTime() : ""
        this.ConvertToModel();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  ConvertToModel() {
    this.modelJobSeeker = new ModelJobSeeker();

    this.modelJobSeeker.About = this.objJobSeeker?.About;
    this.modelJobSeeker.DOB = this.objJobSeeker?.DOB;
    this.modelJobSeeker.Email = this.objJobSeeker?.Email;
    this.modelJobSeeker.ExperienceId = this.objJobSeeker?.Experience._id;
    this.modelJobSeeker.Gender = this.objJobSeeker?.Gender;
    this.modelJobSeeker.JobTitle = this.objJobSeeker?.JobTitle;
    this.modelJobSeeker.Name = this.objJobSeeker?.Name;
    this.modelJobSeeker.Phone = this.objJobSeeker?.Phone;
    this.modelJobSeeker.QualificationId = this.objJobSeeker?.Qualification._id;
    this.modelJobSeeker.SocialFacebook = this.objJobSeeker?.SocialFacebook;
    this.modelJobSeeker.SocialGooglePlus = this.objJobSeeker?.SocialGooglePlus;
    this.modelJobSeeker.SocialLinkedin = this.objJobSeeker?.SocialLinkedin;
    this.modelJobSeeker.SocialTwitter = this.objJobSeeker?.SocialTwitter;
    this.modelJobSeeker.Website = this.objJobSeeker?.Website;
    this.modelJobSeeker._id = this.objJobSeeker?._id;
    this.modelJobSeeker.CountryId = this.objJobSeeker?.Country._id;
    this.modelJobSeeker.CityId = this.objJobSeeker?.City._id;
    this.modelJobSeeker.Languages = this.objJobSeeker.Languages.map(({ _id }) => _id);

    if (this.modelJobSeeker.DOB) {
      var d = new Date(this.objJobSeeker.DOB);
      this.modelDOB = {
        year: d.getUTCFullYear(), month: d.getUTCMonth() + 1
        , day: d.getUTCDate()
      };
    }
    this.onCountrySelect();
  }
  FormatString(value: string, label: string) {
    if (!value)
      return label + " N/A";

    if (value == "")
      return label + " N/A";

    return value
  }
  FormateGender() {
    var value = this.objJobSeeker?.Gender;
    if (value == null)
      return "N/A";

    switch (value) {
      case 0:
        return "N/A";
        break;
      case 1:
        return "M";
        break;
      case 2:
        return "F";
        break;
    }

    return "N/A";
  }
  FormatLocation() {
    if (this.objJobSeeker?.Country?.Name == null || this.objJobSeeker?.City.Name == null)
      return "Country N/A, City N/A";

    if (this.objJobSeeker?.Country?.Name == '' || this.objJobSeeker?.City.Name == '')
      return "Country N/A, City N/A";

    return this.objJobSeeker?.Country?.Name + ", " + this.objJobSeeker?.City.Name;
  }
  FormatGenericSubItem(obj: cGenericSubItem) {
    if (!obj || obj == null)
      return 'N/A';

    if (obj?.Name == null || obj?.Name == "")
      return 'N/A';

    return obj?.Name;
  }
  FormatLanguage() {
    if (!this.objJobSeeker?.Languages)
      return 'N/A';

    if (this.objJobSeeker?.Languages.length == 0)
      return 'N/A';

    const lst = this.objJobSeeker?.Languages.map(({ Name }) => Name);
    return lst.join();
  }
  FormatAge() {
    if (!this.objJobSeeker?.DOB)
      return "N/A";

    var dob = this.objJobSeeker?.DOB;
    var today = new Date();
    var birthDate = new Date(dob);

    if (birthDate.getFullYear() == 1)
      return "N/A";

    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age + " Years";
  }
  FormateSocial(value: string) {
    if (value == null || value == "")
      return "N/A";

    return value;
  }
  onEditDescription() {
    this.IsEditDescription = true;
  }
  onCancelDescription() {
    this.IsEditDescription = false;
    //this.modelJobSeeker.About = this.objJobSeeker.About;
    this.ConvertToModel();
  }
  onSaveDescription() {
    this.BLJobSeeker.UpdateDescription(this.modelJobSeeker).subscribe({
      next: obj => {
        this.loadData();
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.IsEditDescription = false;
  }
  onEditProfile() {
    this.IsEditProfile = true;
    if (this.modelDOB.year == 1) {
      this.modelDOB = null;
    }
  }
  onCancelProfile() {
    this.IsEditProfile = false;
    this.ConvertToModel();
    // this.modelJobSeeker.DOB = this.objJobSeeker.DOB;
    // this.modelJobSeeker.ExperienceId = this.objJobSeeker.Experience._id;
    // this.modelJobSeeker.QualificationId = this.objJobSeeker.Qualification._id;
    // this.modelJobSeeker.Languages = this.objJobSeeker.Languages.map(({ _id }) => _id);
  }
  onSaveProfile() {
    this.BLJobSeeker.UpdateProfile(this.modelJobSeeker).subscribe({
      next: obj => {
        this.loadData();
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
    this.IsEditProfile = false;
  }
  onDOBSelect(param) {
    this.modelJobSeeker.DOB = new Date(param.year, param.month - 1, param.day);
  }
  UpdateInfo(content) {
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  modelSaveBtn(obj) {
    this.BLJobSeeker.UpdateInfo(this.modelJobSeeker).subscribe({
      next: obj => {
        this.loadData();
        this.modalService.dismissAll();
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  onCancelInfo() {
    this.ConvertToModel();
    this.modalService.dismissAll();
  }
  OpenCertificate(content, IsCreate: boolean, obj: ResponseResumeCertification = new ResponseResumeCertification()) {

    this.modelIsCreate = IsCreate;
    this.modelNameEdited = "";
    this.modelCertificate = obj;
    this.modelEduStart = null;
    this.FileToUpload = null;

    if (obj.StartDate) {
      var s = new Date(obj.StartDate);
      if (obj.StartDate) {
        this.modelEduStart = {
          year: s.getUTCFullYear(), month: s.getUTCMonth() + 1
          , day: s.getUTCDate()
        };
      }
    }
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  OpenEducation(content, IsCreate: boolean, name: string, title: string, type: number, obj: ResponseResumeItem = new ResponseResumeItem()) {

    this.modelIsCreate = IsCreate;
    this.modelNameEdited = name;
    this.modelTitle = title;
    this.modelEducation = obj;
    this.modelIsEducation = type == 1;
    this.modelIsWork = type == 2;
    this.modelIsExtra = type == 3;
    this.modelEduStart = null;
    this.modelEduEnd = null;

    if (obj.StartDate) {
      var s = new Date(obj.StartDate);
      if (obj.StartDate) {
        this.modelEduStart = {
          year: s.getUTCFullYear(), month: s.getUTCMonth() + 1
          , day: s.getUTCDate()
        };
      }
    }
    if (obj.EndDate) {
      if (obj.EndDate.toString() != "0001-01-01T00:00:00Z") {
        var e = new Date(obj.EndDate);
        if (obj.EndDate) {
          this.modelEduEnd = {
            year: e.getUTCFullYear(), month: e.getUTCMonth() + 1
            , day: e.getUTCDate()
          };
        }
      }
    }
    this.modalService.open(content, { size: 'lg', backdropClass: 'light-blue-backdrop', centered: true });
  }
  onCerttartSelect(param) {
    this.modelCertificate.StartDate = new Date(param.year, param.month - 1, param.day + 1);
  }
  onEduStartSelect(param) {
    this.modelEducation.StartDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelEducation.EndDate.toString() != "0001-01-01T00:00:00Z") {
      this.IsDatesCorrect = this.CompareDates(this.modelEducation.StartDate, this.modelEducation.EndDate);
    }
  }
  onEduEndSelect(param) {
    this.modelEducation.EndDate = new Date(param.year, param.month - 1, param.day + 1);
    if (this.modelEducation.EndDate.toString() != "0001-01-01T00:00:00Z") {
      this.IsDatesCorrect = this.CompareDates(this.modelEducation.StartDate, this.modelEducation.EndDate);
    }
  }
  modelSaveCertBtn(obj) {
   
    if (this.FileToUpload != null) {
      if (this.FileToUpload.type != "application/pdf"
        && this.FileToUpload.type != "application/doc"
        && this.FileToUpload.type != "application/docx"
        && this.FileToUpload.type != "image/png"
        && this.FileToUpload.type != "image/jpeg"
        && this.FileToUpload.type != "image/jpg") {
        this.message.Error(this.SeekerCertifiateFileError);
        this.BLServiceShowMessage.sendMessage(this.message);
        return;
      }
      this.isClicked = true;
      const formData = new FormData();

      formData.append('file', this.FileToUpload, this.FileToUpload.name);
      this.BLServiceFile.UploadFile(formData).subscribe({
        next: obj => {
          this.modelCertificate.CertificatePath = obj;
          this.UpdateCertificateDB();
        }
      });
    }
    else{
      this.UpdateCertificateDB();
    }
  }
  UpdateCertificateDB() {
    if (this.modelIsCreate) {
      this.BLJobSeeker.AddCertification(this.modelCertificate).subscribe({
        next: obj => {
          this.loadData();
          this.modalService.dismissAll();
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
          this.isClicked = false;
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
          this.isClicked = false;
        }
      });
    }
    else {
      this.BLJobSeeker.UpdateCertification(this.modelCertificate).subscribe({
        next: obj => {
          this.loadData();
          this.modalService.dismissAll();
          this.message.Success("Saved successfully.");
          this.BLServiceShowMessage.sendMessage(this.message);
        },
        error: err => {
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);
        }
      });
    }
  }
  modelSaveEduBtn(obj) {

    if (this.modelIsEducation) //education
    {
      if (this.modelIsCreate) {
        this.BLJobSeeker.AddEducation(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
      else {
        this.BLJobSeeker.UpdateEducation(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
    }
    else if (this.modelIsWork) //WorkExperience
    {
      if (this.modelIsCreate) {
        this.BLJobSeeker.AddWorkExperience(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
      else {
        this.BLJobSeeker.UpdateWorkExperience(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
    }
    else if (this.modelIsExtra) //ExtraCurricular
    {
      if (this.modelIsCreate) {
        this.BLJobSeeker.AddExtraCurricular(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
      else {
        this.BLJobSeeker.UpdateExtraCurricular(this.modelEducation).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Saved successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      }
    }
  }
  onCertRemove(Id: string) {
    this.confirmationDialogService.confirm("Are you sure you want to remove this?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLJobSeeker.RemoveCertification(Id).subscribe({
          next: obj => {
            this.loadData();
            this.modalService.dismissAll();
            this.message.Success("Removed successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
          },
          error: err => {
            this.message.Error(err);
            this.BLServiceShowMessage.sendMessage(this.message);
          }
        });
      });
  }
  onEduRemove(Id: string, type: number) {
    this.confirmationDialogService.confirm("Are you sure you want to remove this?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        if (type == 1) //education
        {
          this.BLJobSeeker.RemoveEducation(Id).subscribe({
            next: obj => {
              this.loadData();
              this.modalService.dismissAll();
              this.message.Success("Removed successfully.");
              this.BLServiceShowMessage.sendMessage(this.message);
            },
            error: err => {
              this.message.Error(err);
              this.BLServiceShowMessage.sendMessage(this.message);
            }
          });
        } else if (type == 2)//workexperience
        {
          this.BLJobSeeker.RemoveWorkExperience(Id).subscribe({
            next: obj => {
              this.loadData();
              this.modalService.dismissAll();
              this.message.Success("Removed successfully.");
              this.BLServiceShowMessage.sendMessage(this.message);
            },
            error: err => {
              this.message.Error(err);
              this.BLServiceShowMessage.sendMessage(this.message);
            }
          });
        }
        else if (type == 3)//Extra-Curricular
        {
          this.BLJobSeeker.RemoveExtraCurricular(Id).subscribe({
            next: obj => {
              this.loadData();
              this.modalService.dismissAll();
              this.message.Success("Removed successfully.");
              this.BLServiceShowMessage.sendMessage(this.message);
            },
            error: err => {
              this.message.Error(err);
              this.BLServiceShowMessage.sendMessage(this.message);
            }
          });
        }
      });
  }
  onAddFavourite(Id: string) {

    this.BLFavourite.Create(Id).subscribe({
      next: obj => {
        this.message.Success("Saved successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.LoadFav();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  onRemoveFavourite(Id: string) {
    this.confirmationDialogService.confirm("Are you sure you want to remove from the shortlist?")
      .then((confirmed) => {
        if (!confirmed)
          return;
        this.BLFavourite.DeActivateByJobId(Id).subscribe({
          next: response => {
            this.message.Success("Removed Successfully.");
            this.BLServiceShowMessage.sendMessage(this.message);
            this.LoadFav();
          },
          error: err => this.message.Error(err)
        });
      });
  }
  UploadStatus(status) {
    /// this.IsUploading = status;
  }
  OnFileUpload(filename) {
    //this.FileName = filename;
    //this.modelObj.CompanyLogo = filename;
  }
  handleFileInput(files: FileList, type) {

    const formData = new FormData();
    formData.append('logo', files.item(0), files.item(0).name);
    this.BLServiceFile.UploadFile(formData).subscribe({
      next: obj => {
        if (type == 1)//cover letter
        {
          //this.modelJobSeeker.CoverLetterFile == obj;
          this.saveFile(obj, 1);
        }
        else if (type == 2)//Resume
        {
          //this.modelJobSeeker.ResumeFile == obj
          this.saveFile(obj, 2);
        }
        else if (type == 3)//Profile Picture
        {
          // this.modelJobSeeker.ProfilePicture == obj
          this.saveFile(obj, 3);
        }
      }
      ,
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }

  saveFile(fileName, type) {
    this.BLJobSeeker.UploadFile(fileName, type).subscribe({
      next: obj => {
        this.loadData();
        this.message.Success("Uploaded successfully.")
        this.BLServiceShowMessage.sendMessage(this.message);
      }
      ,
      error: err => this.message.Error(err)
    });
  }
  scrollToElement($element): void {
    
    $element.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
  }
  IsValidModel() {
    return (this.modelJobSeeker.CountryId != '' && this.modelJobSeeker.CountryId != '-1') &&
      (this.modelJobSeeker.CityId != '' && this.modelJobSeeker.CityId != '-1') &&
      (this.modelJobSeeker.Gender != 0 && this.modelJobSeeker.Gender != -1)
  }
  public OnFileChanged(files) {
    this.FileToUpload = <File>files[0];
  }
  onRequestContactPermission(Id: string) {
    var model = new ModelId();
    model.Id = Id;
    this.BLJobSeeker.ContactPermissionRequest(model).subscribe({
      next: obj => {
        this.message.Success("Request sent successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  Import()
  {
    this.BLJobSeeker.ImportCertification().subscribe({
      next: obj => {
        this.message.Success("Request sent successfully.");
        this.BLServiceShowMessage.sendMessage(this.message);
        this.loadData();
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
}
