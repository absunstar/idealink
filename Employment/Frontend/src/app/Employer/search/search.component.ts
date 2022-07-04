import { Component, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { cGenericSubItem, cGenericIdNameItem } from 'src/app/interface/Response/GenericIdName.class';
import { cJobList, cJobItem } from 'src/app/interface/Response/Job.class';
import { Page } from 'src/app/common/pagination/page';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceQualification } from 'src/app/services/qualification.service';
import { ServiceYearsOfExperience } from 'src/app/services/years-of-experience.service';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { ServiceLanguages } from 'src/app/services/languages.service';
import { ModelJobSearch } from 'src/app/interface/Model/ModelJobSearch.class';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceIndustry } from 'src/app/services/industry.service';
import { ServiceJobFields } from 'src/app/services/job-fields.service';
import { TranslateService } from '@ngx-translate/core';
import { ServiceCountry } from 'src/app/services/country.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { stringify } from '@angular/compiler/src/util';
import { PagerService } from '../pager.service';
import { Constants } from 'src/app/constants';
// import { FilterPipe } from '../search/filter.pipe';
declare function  applyFilter(f:any,g:any):any
// const applyFilter = (data, filter) => data.filter(obj =>
//   Object.entries(filter).every(([prop, find]) => find.includes(obj[prop]))
// );

// // demo
// var users = [{name: 'John',email: 'johnson@mail.com',age: 25,address: 'USA'},{name: 'Tom',email: 'tom@mail.com',age: 35,address: 'England'},{name: 'Mark',email: 'mark@mail.com',age: 28,address: 'England'}];var filter = {address: ['England'], name: ['Mark'] };
// var filter = {address: ['England'], name: ['Mark'] };

// console.log(applyFilter(users, filter));

declare var $: any;
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent extends baseComponent implements OnInit {
  @ViewChildren('myCheckbox') private myCheckboxes :any;

  currentData;
  searchText: string;
  searchTextC:string
  stepTwo: boolean;
  currentDataOG;
  LkupJobFieldExtra;
  hideCheck:boolean = false;
  less1: number = 0;
  limts1: number = 5;
  less2: number = 0;
  limts2: number = 5;
  less3: number = 0;
  limts3: number = 5;

  LkupQualification: cGenericSubItem[];
  LkupExperience: cGenericSubItem[];
  LkupLanguage: cGenericSubItem[];
  LkupJobField: cGenericIdNameItem[];
  LkupIndustry: cGenericSubItem[];
  LkupCountry: cGenericIdNameItem[];
  LkupCity: cGenericSubItem[];
  contactForm:FormGroup;
  jobsearchCity:string;

  modelSearch: ModelJobSearch = new ModelJobSearch();
  lstResult: cJobList;
  page: Page<cJobItem> = new Page();
  lstJobs: any;
  hideCheck1: boolean = false;
  hideCheck2: boolean = false;
  hideCheck3: boolean= false;
  arrayCheck: any=[];
  arrayCheck1: any=[];
  arrayCheck2: any=[];
  arrayCheck3: any=[];
  arrayCheck4: any=[];
  req:any = {
    CompanyId:[],
    ExperienceId:[],
    IndustryId:[],
    JobFieldId:[],
    Qualificationid:[],
    filterText:'',
    cityId:''
  };
  request: any = {};
  recentSkillSearch: any = [];
  recentLocSearch: any = [];
  // recentLoc1: string;
  // recentSkill1:string;
  // lastSearchArraySkill : any = [];
  // lastSearchArrayLoc : any = [];
  onlySkill :any = [];
  onlySkillHTM :any = [];
  OnlylocationHTM :any = [];
  Onlylocation : any = [];
  currentDatas: any;
  totalfilterCheck: any;
  OnlylocationRecent: any;
  onlySkillRecent: any;
  lessText: any = "less text";
  LkupExperienceExtra: cGenericSubItem[];
  LkupIndustryExtra: cGenericSubItem[];
  sampleInput: string;
  searchComp:any=[]
  isReadMore:boolean=true
  SCLTH:number=5;
  coun:any = []
  Refbutton:boolean=false
  JobCumpanyC=[]
  JobFieldC:any=[]
  IndustryC:any=[]
  ExperienceC:any=[]
  Qualification:any=[]
  allData:any=[]
  allDataBackup:any=[]


  allItems: any[];

  // pager object
  pager: any = {};

  // paged items
  pagedItems: any=[];

  constructor(
    private BLCountry: ServiceCountry,
    private fb :FormBuilder,
    private BLJob: ServiceJob,
    BLServiceShowMessage: ServiceShowMessage,
    BLJobLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private BLQualification: ServiceQualification,
    private BLExperience: ServiceYearsOfExperience,
    private BLIndustry: ServiceIndustry,
    private BLJobFields: ServiceJobFields,
    private BLFavourite: ServiceFavourite,
    private paginationService: CustomPaginationService,
    private BLLanguages: ServiceLanguages,
    private http: HttpClient,
    private PagerService:PagerService,
    private cookies :CookieService) {
    super(BLServiceShowMessage, BLJobLoginUser,BLTranslate)
    $('.preloaderSecond').css('display','block')
  }
  async ngOnInit() {
    this.company();
    this.loadData();
    console.log(this.LkupIndustry);
    this.hideCheck = false;
    this.createForm();
    this.companyViewInit()
    $('.preloaderSecond').css('display','block')
    let priv =  this.cookies.get('priv') ? JSON.parse(this.cookies.get('priv')) : {cityId:[],filterText:''}
    let privT =  this.cookies.get('privT')
    console.log(priv);
    if((priv.cityId!='' || priv!=undefined|| priv.cityId!=null || priv.filterText!='') && privT=="true"){
      this.losding()
      this.cookies.set('privT',JSON.stringify(false))
      
    }else{
      this.cookies.set('privT',JSON.stringify(false))
      priv={cityId:[],filterText:''}
    }
   
    let url = Constants.apiRoot+ "/Job/Search";
    $('#Lod').show()
      this.http.post( url,priv).subscribe((res)=>{
        this.currentData = res;
        this.Refbutton=true
        this.allData=res
        this.allDataBackup=res
        $('#Lod').hide()
        // this.companyViewInit()
        console.log(this.currentData);
        this.cookies.set('priv',JSON.stringify({}))
       
        $('.preloaderSecond').css('display','block')
        if(this.currentData.totalCount==0){
          $('.preloaderSecond').css('display','block')
          this.http.post( Constants.apiRoot+ '/Job/SearchCompany',Object({CompanyId:''})).subscribe({
            next: (lst) => {
              this.allData=lst
              this.allDataBackup=lst
              this.Refbutton=false
              this.currentData = lst;
              this.lstResult = Object.assign(lst);
              this.page.pageable.pageSize = Object.assign(lst).pageSize;
              this.page.totalElements = Object.assign(lst).totalCount;
              this.page.content = Object.assign(lst).lstResult;
              this.cookies.set('priv',JSON.stringify({}))
              this.setPage(1);
              $('.preloaderSecond').hide()
             },
            error: err => {
              this.message.Error(err);
              this.BLServiceShowMessage.sendMessage(this.message);
              $('.preloaderSecond').hide()
            }
            
          })
        }else{
          this.onsubCountfilter(res)
          $('.preloaderSecond').css('display','block')
          this.onsubCountremoveFilter()
          this.setPage(1);
        }
        // console.log(res);

      
    })



    // this.BLTranslate.get("jobsearchCity").subscribe(res => { this.jobsearchCity = res; });
    // console.log(this.LkupJobField);

    // this.physicians = merge(this.physicianInfoForm.controls['firstname'].valueChanges,
    //     this.physicianInfoForm.controls['lastname'].valueChanges).pipe(debounceTime(300),map(v => {
    // return {
    //     "firstname": this.physicianInfoForm.controls['firstname'].value,
    //     "lastname": this.physicianInfoForm.controls['lastname'].value
    // }
    //   })
    //     );
  }
  losding(){
    setTimeout(() => {
      $('.preloaderSecond').css('display','block')
    }, 3000);
  }

  filterMenu(){
    var set=!1,set1=!1,set2=!1,set3=!1,set4=!1,set5=!1;
    setTimeout(() => {
      $('.preloaderSecond').css('display','block')
    }, 1800);
    this.BLCountry.getListActive().subscribe({
      next: lst => {
        this.LkupCountry = lst;
        this.onCountrySelect();
        set=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set=true
      }
    });
    this.BLIndustry.getListActive().subscribe({
      next: lst => {
        this.LkupIndustry = lst;  
          setTimeout(() => {
          this.allcount(this.LkupIndustry,this.IndustryC)   
          },1000);
          set1=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set1=true
      }
    });
    this.BLJobFields.getListActive().subscribe({
      next: lst => {
        this.LkupJobField = lst;
        setTimeout(() => {
          this.allcount( this.LkupJobField,this.JobFieldC)   
          }, 700);
          set2=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set2=true
      }
    });
    this.BLQualification.getListActive().subscribe({
      next: lst => {
        this.LkupQualification = lst;
        setTimeout(() => {
          this.allcount(this.LkupQualification,this.Qualification)   
          }, 800);
          set3=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set3=true
      }
    });
    this.BLExperience.getListActive().subscribe({
      next: lst => {
        this.LkupExperience = lst;
        console.log(this.LkupExperience)
        setTimeout(() => {
          this.allcount(this.LkupExperience,this.ExperienceC)   
          }, 900);
          set4=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set4=true
      }
    });
    this.BLLanguages.getListActive().subscribe({
      next: lst => {
        this.LkupLanguage = lst;
        set5=true
      },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
        set5=true
      }
    });
   
    setTimeout(() => {
      $('.preloaderSecond').hide()
    }, 2500);
  }
  // hidload(set,set1,set2,set3,set4,set5){
  //   if(set && set1 && set2 && set3 && set4 && set5){
  //     $('.preloaderSecond').hide()
  //   }
  // }
  companyViewInit(){
    $('.preloaderSecond').css('display','block')
    var domD=[]
    let priv =  {CompanyId:''}
    let currentData =Object();
    let url = Constants.apiRoot+ "/Job/SearchCompany";
    this.http.post( url,priv).subscribe((res)=>{
      currentData =Object(res);
      $('.preloaderSecond').hide()
      this.allData=res
      this.allDataBackup=res
      for(let i=0; i<currentData.lstResult.length; i++){
        if( currentData.lstResult[i].Company!=''){
          if(!domD.includes(currentData.lstResult[i].Company)){
            this.searchComp.push({Name:currentData.lstResult[i].Company})
            // this.totoljob(currentData.lstResult[i].Company)
          }
          domD.push(currentData.lstResult[i].Company)
        }
        
      // 
    
        
      }
      for (let i = 0; i < currentData.lstResult.length; i++) {
        this.JobFieldC.push(currentData.lstResult[i].JobField)
        this.JobCumpanyC.push(currentData.lstResult[i].Company)
        this.IndustryC.push(currentData.lstResult[i].Industry)
        this.ExperienceC.push(currentData.lstResult[i].Experience)
        this.Qualification.push(currentData.lstResult[i].Qualification)
        
      }
      this.coun=[]
      setTimeout(() => {
        this.searchComp.forEach(element => {
          element.IsActive=true
        });
        this.allcount(this.searchComp,this.JobCumpanyC)
         
        }, 300);
      // console.log(this.searchComp);

      // this.currentData = currentData;
      // this.setPage(1);
      this.filterMenu()
  })

  // setTimeout(() => {
  //   console.log(this.JobFieldC);
  // }, 800);
  

  }

  // totoljob(even:any){
  //   // console.log(this.JobCumpanyC);
    
  //   this.JobCumpanyC.forEach(data => {
  //     if(data==even){
  //       this.coun.push(data);  
  //     }
  //   });

  //   this.searchComp.forEach(data => {
  //    var t= Object(data)
  //     t.Count=this.allc(t.Name,)
  //   });

  // }
  allc(d){
    var counts = 0;
    for (let i = 0; i < this.coun.length; i++) {
     if(this.coun[i]==d){
        counts ++
     } 
    }
    
    return counts
  }

  allcount(LkupJobField,JobFieldC){
    // setTimeout(() => {
    // console.log(this.JobFieldC);
      for (let i = 0; i < LkupJobField.length; i++) {
        LkupJobField[i].Count=Number(this.LkupJobFieldcount(LkupJobField[i].Name,JobFieldC))
      }
    // }, 800);
  
  }

  LkupJobFieldcount(e,JobFieldC){
    var counts = 0;
    for (let i = 0; i < JobFieldC.length; i++) {
     if(JobFieldC[i]==e){
        counts ++
     } 
    }
    return counts
    
  }
  public getNextPage(): void {
    this.page.pageable = this.paginationService.getNextPage(this.page);
    this.loadData();
  }

  public getPreviousPage(): void {
    this.page.pageable = this.paginationService.getPreviousPage(this.page);
    this.loadData();
  }
  public getloadPageCurrent(): void {
    this.loadData();
  }
  FilterData(){
    this.page.pageable.pageCurrent = 1;
    this.loadData();
  }
  loadData() {
    this.modelSearch.CurrentPage = this.page.pageable.pageCurrent;
    this.BLJob.Search(this.modelSearch).subscribe({
      next: lst => {
        this.lstResult = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
       },
      error: err => {
        this.message.Error(err);
        this.BLServiceShowMessage.sendMessage(this.message);
      }
    });
  }
  FormateGender(value) {
    
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
  FormatGenericSubItem(obj: cGenericSubItem) {
    if (!obj || obj == null)
      return 'N/A';

    if (obj?.Name == null || obj?.Name == "")
      return 'N/A';

    return obj?.Name;
  }
submit(){  
}
onCountrySelect() {
  this.LkupCity = [];

  var sub = this.LkupCountry?.find(x => x._id == this.modelSearch.CountryId)?.subItems;
  if (!sub || sub.length == 0)
    return;

  this.LkupCity = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
  this.FilterData();
}

// loc first alpha capital
// trigger(event) {
//   const inputValue = event.target.value;
//   const result = inputValue.charAt(0).toUpperCase() + inputValue.slice(1);
//   this.sampleInput = result;
//   console.log(this.sampleInput);
// }

// code start
createForm() {
  // console.log(this.cookies.get('Onlyloc'));
  this.Onlylocation =  this.cookies.get('Onlyloc') ? JSON.parse(this.cookies.get('Onlyloc')) : []
  this.OnlylocationHTM =  this.cookies.get('Onlyloc') ? JSON.parse(this.cookies.get('Onlyloc')) : []
  // this.onlySkillRecent 
  this.onlySkill = this.cookies.get('skillC')  ? JSON.parse(this.cookies.get('skillC')) : []
  this.onlySkillHTM = this.cookies.get('skillC')  ? JSON.parse(this.cookies.get('skillC')) : []
  this.cookies.set('Onlyloc',JSON.stringify(this.OnlylocationHTM))
  this.cookies.set('skillC',JSON.stringify(this.onlySkillHTM))
  this.contactForm = this.fb.group({
    skills: '',
    location : ''
  });
}
get k() { return this.contactForm.controls; }
// submit on search
onSubmit(data) {
  console.log(data.value);
  var req=Object()
  let url = Constants.apiRoot+ "/Job/Search";
  if(this.contactForm.get('skills').value != '' ||  this.contactForm.get('location').value != ''){
   let getONlyloc= this.cookies.get('Onlyloc');
   let getskills = this.cookies.get('skillC');
    const l= Object(JSON.parse( getONlyloc))
    const k= Object(JSON.parse( getskills))
    if(l.length>0 || k.length>0){
      if(this.contactForm.get('skills').value != ''){
        if(!k.includes(this.contactForm.get('skills').value)){
          this.onlySkill.push(this.contactForm.get('skills').value)
          this.cookies.set('skillC',JSON.stringify(this.onlySkill))
        }
      }
      if(this.contactForm.get('location').value != ''){
        if(!l.includes(this.contactForm.get('location').value)){
          this.Onlylocation.push(this.contactForm.get('location').value)
          this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
        }
      }
    }else{
      if(this.contactForm.get('skills').value != ''){
        this.onlySkill.push(this.contactForm.get('skills').value.toString())
        this.cookies.set('skillC',JSON.stringify(this.onlySkill))
      }
      if(this.contactForm.get('location').value != ''){
        this.Onlylocation.push(this.contactForm.get('location').value.toString())
        this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
      }
    }

    //  console.log(this.Onlylocation,this.onlySkill);
    var laction= ''
    var lactionskil=''
    if( this.contactForm.get('location').value.toString()!=''&&this.contactForm.get('location').value.toString()!=null){
      laction= this.contactForm.get('location').value.split(',').map( a => a.charAt(0).toUpperCase() + a.substr(1))
    }
    if( this.contactForm.get('skills').value.toString()!=''&&this.contactForm.get('skills').value.toString()!=null){
      lactionskil= this.contactForm.get('skills').value.toLowerCase()
    }
   req.filterText = lactionskil
   req.cityId = laction
    $('.preloaderSecond').css('display','block')
   this.cookies.set('priv',JSON.stringify(req))
   this.http.post( url,req).subscribe((res)=>{
      this.currentData=this.currentDataOG = res;
      this.allData=res
      this.allDataBackup=res
      if(this.k.skills.value!=''){
        this.Refbutton=true
      }
      if(this.k.location.value!=''){
        this.Refbutton=true
      }
      console.log(this.currentDataOG);
      this.onsubCountfilter(this.currentData)

      // this.setPage(1);
      // this.filte(this.req)
      console.log(this.req);
      
      this.onsubCountremoveFilter()
      $('.preloaderSecond').hide()
   })
  }else{
  //   this.req.cityId = ''
  //   this.req.filterText = ''
  //   this.http.post( url,this.req).subscribe((res)=>{
  //     this.currentData=this.currentDataOG = s;
  //     console.log(this.currentDataOG);
  //     this.setPage(1);
  //  })
  }




  // if(this.contactForm.get('skills').value != '' &&  this.contactForm.get('location').value != '') {
  //   // this.stepTwo = true;
  //   let url = Constants.apiRoot+ "/Job/Search";
  //   this.request.cityId = this.contactForm.get('location').value
  //   this.request.filterText = this.contactForm.get('skills').value
  //   console.log(this.request);
  //   if(this.onlySkill.length < 3) {
  //     this.onlySkill.push(this.contactForm.get('skills').value.toString())
  //     this.cookies.set('skillC',JSON.stringify(this.onlySkill))
  //     console.log(this.onlySkill);
  //   }
  //    else {
  //     this.onlySkill.splice(0,1);
  //     this.onlySkill.push(this.contactForm.get('skills').value.toString())
  //     this.cookies.set('skillC',JSON.stringify(this.onlySkill))
  //   }
  //   if(this.Onlylocation.length < 3) {
  //     this.Onlylocation.push(this.contactForm.get('location').value.toString())
  //     this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
  //     console.log(this.Onlylocation);
  //   } 
  //   else {
  //     this.Onlylocation.splice(0,1);
  //     this.Onlylocation.push(this.contactForm.get('location').value.toString())
  //     this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
  //   }
  //   this.http.post( url,this.request).subscribe((res)=>{
  //     this.currentData=this.currentDataOG = res;
  //     console.log(this.currentDataOG);
  //   })
  // }
  // else if(this.contactForm.get('skills').value != ''){
  //   // this.stepTwo = true;
  //   if(this.onlySkill.length < 3) {
  //     this.onlySkill.push(this.contactForm.get('skills').value.toString())
  //     this.cookies.set('skillC',JSON.stringify(this.onlySkill))
  //     console.log(this.onlySkill);
  //   }
  //    else {
  //     this.onlySkill.splice(0,1);
  //     this.onlySkill.push(this.contactForm.get('skills').value.toString())
  //     this.cookies.set('skillC',JSON.stringify(this.onlySkill))
  //   }
  //   let url = Constants.apiRoot+ "/Job/Search";
  //   this.request.filterText = this.contactForm.get('skills').value
  //   // this.cookies.set('skill',this.contactForm.get('skills').value)
  //   console.log(this.request);
  //   this.http.post( url,this.request).subscribe((res)=>{
  //     this.currentData = this.currentDataOG = res;
  //     console.log(this.currentDataOG);
  //   })
  // }
  // else if(this.contactForm.get('location').value != '') {
  //   // this.stepTwo = true;
  //   if(this.Onlylocation.length < 3) {
  //     this.Onlylocation.push(this.contactForm.get('location').value.toString())
  //     this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
  //     console.log(this.Onlylocation);
  //   } 
  //   else {
  //     console.log("2nd if",this.Onlylocation);
  //     this.Onlylocation.splice(0,1);
  //     this.Onlylocation.push(this.contactForm.get('location').value.toString())
  //     this.cookies.set('Onlyloc',JSON.stringify(this.Onlylocation))
  //   }
  //   let url = Constants.apiRoot+ "/Job/Search";
  //   this.request.cityId = this.contactForm.get('location').value
  //   console.log(this.request);
  //   this.http.post( url,this.request).subscribe((res)=>{
  //     this.currentData = this.currentDataOG = res;
  //     console.log(this.currentData);
  //     // console.log(res);
  //   })
  // }
  // else {
  //   this.ngOnInit();
  // }
}

onsubCountfilter(D:any){
  this.allData=D
  this.allDataBackup=D
  this.JobFieldC=[]
  this.JobCumpanyC=[]
  this.IndustryC=[]
  this.ExperienceC=[]
  this.Qualification=[]
  
  for (let i = 0; i < D.lstResult.length; i++) {
    this.JobFieldC.push(D.lstResult[i].JobField)
    this.JobCumpanyC.push(D.lstResult[i].Company)
    this.IndustryC.push(D.lstResult[i].Industry)
    this.ExperienceC.push(D.lstResult[i].Experience)
    this.Qualification.push(D.lstResult[i].Qualification)
    
  }
  setTimeout(() => {
    this.searchComp.forEach(element => {
      element.IsActive=true
    });
    this.allcount(this.searchComp,this.JobCumpanyC)    
    }, 300);
    setTimeout(() => {
      this.allcount(this.LkupIndustry,this.IndustryC)   
      },1000);
    setTimeout(() => {
      this.allcount( this.LkupJobField,this.JobFieldC)   
      }, 700);
    setTimeout(() => {
      this.allcount(this.LkupQualification,this.Qualification)   
      }, 800);
    setTimeout(() => {
      this.allcount(this.LkupExperience,this.ExperienceC)   
      }, 900);
     
}

onsubCountremoveFilter() {
  setTimeout(() => {
    $('.preloaderSecond').css('display','block')
  }, 500);
  // this.ngOnInit();
  // $('input').parents('mat-checkbox').removeClass('mat-checkbox-checked')
  // $('input').parents('mat-checkbox').addClass('cdk-focused cdk-program-focused')
  // $('input').parents('mat-checkbox').attr('aria-checked="false"')
  // $("input:checkbox:not(:checked)").prop('ariaChecked', false)
  // $("input:checkbox:not(:checked)").prop('checked', false)
  // $("input:checkbox:not(:checked)").click()


  
  this.myCheckboxes.checked = false;
  // this.req = {};    
  // console.log(this.req)
  // let tryCheck = document.querySelector('.xyz')
  // tryCheck.forEach(i => {
  //   console.log(i)
  //   // console.log(i.target.checked)
  // })
  // this.contactForm.controls.skills.setValue('');
  // this.contactForm.controls.location.setValue('');
  // this.req = {
  //   CompanyId:[],
  //   ExperienceId:[],
  //   IndustryId:[],
  //   JobFieldId:[],
  //   Qualificationid:[],
  //   filterText:'',
  //   cityId:''
  // };
  // this.arrayCheck = [];
  // this.arrayCheck1 = [];
  // this.arrayCheck2 = [];
  // this.arrayCheck3 = [];
  // this.arrayCheck4 = [];
  // this.Refbutton=false
  // this.searchComp=[]
  this.fildsubmintfilte(this.req)
}


//   searchData() {
//     if(this.modelSearch.filterText != '') {
//     let url = Constants.apiRoot+ "/Job/Search";
//       this.http.post( url,{
//         filterText : this.modelSearch.filterText
//       }).subscribe((res)=>{
//         let count = res;
//         console.log(count);
//         console.log(this.lstResult.lstResult[0].Name)
//       })
//     }
//     console.log(this.modelSearch.filterText);
//   }


  // 1
  FieldSearchData(event,id) {
    console.log(event,id);
    
    // this.LkupJobFieldExtra = this.LkupJobField;
    if(event.checked === true) {
     this.arrayCheck.push(id) 
     console.log("TRUEEE",this.arrayCheck)
     this.req.JobFieldId =  this.arrayCheck 
    } else {
      const index = this.arrayCheck.indexOf(id);
      if (index > -1) {
        this.arrayCheck.splice(index, 1);
      }
      // this.arrayCheck.splice(id)
    }
    // console.log(this.arrayCheck.map(v => v.toLowerCase()));
    if(this.contactForm.get('location').value != '') {
    // this.req.cityId = this.sampleInput
    this.req.cityId = this.contactForm.get('location').value.split(',')
    }
    if(this.contactForm.get('skills').value != '') {
    this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    console.log(this.req);
    this.filte(this.req)
    // $('.preloaderSecond').css('display','block')
    // let url = Constants.apiRoot+ "/Job/Search";
    // this.http.post( url,this.req).subscribe((res)=>{
    //   this.currentData = res;
    //   console.log(this.currentData);
    //   this.objectnull(this.req)
    //   this.setPage(1);
    //   $('.preloaderSecond').hide()
      // this.currentData.lstResult =  this.currentData.lstResult.map(item => {
      //   item.checked = false
      // })
      // console.log(this.currentData)
    // })

    // this.totalfilterCheck = this.arrayCheck.length; 
  }
  
  // 2
  FilterIndustryData(event,id) {
    if(event.checked === true) {
       this.arrayCheck1.push(id) 
       console.log(this.arrayCheck1.map(v => v.toLowerCase()))
      // this.arrayCheck1 = id
      this.req.IndustryId =  this.arrayCheck1
      } else {
        const index = this.arrayCheck1.indexOf(id);
      if (index > -1) {
        this.arrayCheck1.splice(index, 1);
      }
        // this.arrayCheck1.splice(id) 
      }
    // var req = {'filterText' : this.contactForm.get('location').value,'IndustryId' :[ this.arrayCheck1 ]}
    if(this.contactForm.get('location').value != '') {
    this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
    }
    if(this.contactForm.get('skills').value != '') {
    this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    this.filte(this.req)
    // $('.preloaderSecond').css('display','block')
    // console.log(this.req);
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe((res)=>{
    //     this.currentData = res;
    //     this.objectnull(this.req)
    //     console.log(this.currentData);
    //     this.setPage(1);
    //     $('.preloaderSecond').hide()
    //     // console.log(res);
    //   })

      // this.totalfilterCheck = this.arrayCheck.length + this.arrayCheck1.length;
  }

  // 3
  FilterExphData(event,id) {
    if(event.checked === true) {
       this.arrayCheck2.push(id)      
      // this.arrayCheck2 = id;
      console.log(this.arrayCheck2)
      this.req.ExperienceId = this.arrayCheck2
      }
      else {
        // this.arrayCheck2.splice(id) 
        const index = this.arrayCheck2.indexOf(id);
        if (index > -1) {
          this.arrayCheck2.splice(index, 1);
        }
      }
    // var req = {'filterText' : this.contactForm.get('location').value,'ExperienceId' :[ this.arrayCheck ]}
    
    if(this.contactForm.get('location').value != '') {
    // this.req.cityId = this.contactForm.get('location').value
    this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
    }
    if(this.contactForm.get('skills').value != '') {
    this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    this.filte(this.req)
    // $('.preloaderSecond').css('display','block')
    // console.log(this.req);
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe((res)=>{
    //     this.currentData = res;
    //     this.objectnull(this.req)
    //     console.log(this.currentData);
    //     this.setPage(1);
    //     $('.preloaderSecond').hide()
    //     // console.log(res);
    //   })
      // this.totalfilterCheck = this.arrayCheck.length + this.arrayCheck1.length + this.arrayCheck2.length;
  }

  // 4
  FilterSearchData(event,id) {
    if(event.checked === true) {
       this.arrayCheck3.push(id) 
       console.log(this.arrayCheck3)
      // this.arrayCheck3 = id
      this.req.Qualificationid = this.arrayCheck3
      this.modelSearch.Qualificationid=this.req.Qualificationid
    }
    else {
      // this.arrayCheck3.splice(id) 
      const index = this.arrayCheck3.indexOf(id);
      if (index > -1) {
        this.arrayCheck3.splice(index, 1);
      }
    }
    // this.req= {'' : this.contactForm.get('location').value,'Qualificationid' :[ id ]}
    // this.req.filterText= this.contactForm.get('location').value;
    if(this.contactForm.get('location').value != '') {
      this.req.CityiId = this.contactForm.get('location').value.split(',')
      // this.req.cityId = this.sampleInput
      }
      if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
      this.modelSearch.filterText=this.req.filterText
      }
    
    console.log(this.req);
    this.filte(this.req)
    // $('.preloaderSecond').css('display','block')
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe({
    //     next: (lst) => {
    //       this.currentData = lst;
    //       this.lstResult = Object.assign(lst);
    //       this.page.pageable.pageSize = Object.assign(lst).pageSize;
    //       this.page.totalElements = Object.assign(lst).totalCount;
    //       this.page.content = Object.assign(lst).lstResult;
    //       this.objectnull(this.req)
    //       this.setPage(1);
    //       $('.preloaderSecond').hide()
    //      },
    //     error: err => {
    //       this.message.Error(err);
    //       this.BLServiceShowMessage.sendMessage(this.message);
    //       $('.preloaderSecond').hide()
    //     }
    //   })
      // this.totalfilterCheck = this.arrayCheck.length + this.arrayCheck1.length + this.arrayCheck2.length + this.arrayCheck3.length;
  }

  //5
  FilterCompny(event,id) {
    console.log(event);
    
    if(event.checked === true) {
      this.arrayCheck4.push(id)   
      this.req.CompanyId =this.arrayCheck4 
    }else {
        // this.arrayCheck2.splice(id) 
        const index = this.arrayCheck4.indexOf(id);
        if (index > -1) {
          this.arrayCheck4.splice(index, 1);
        }
        this.req.CompanyId =this.arrayCheck4
    }
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
      }
    if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    // $('.preloaderSecond').css('display','block')
    console.log(this.req);
    // event.checked=!event.checked
    this.filte(this.req)
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe((res)=>{
    //     this.currentData = res;
    //     console.log(this.currentData);
    //     this.objectnull(this.req)
    //     // console.log(res);
    //     this.setPage(1);
    //     $('.preloaderSecond').hide()
    //   })

      // this.totalfilterCheck = this.arrayCheck.length + this.arrayCheck1.length + this.arrayCheck2.length;
  }

  search(event) {
    console.log(event.target.value);
    event.target.value
    // let url = Constants.apiRoot+ "/Job/ForSearchValidation"
    let url = Constants.apiRoot+ "/Job/ForSearchValidation"
    this.http.post(url ,{
      filterTextValidation : this.contactForm.get('skills').value
    }).subscribe(res => {
      var alllist=Object(res)
      var c,j,n;
      this.currentDatas = [];
      for(let i = 0; i < alllist.lstResult.length; i++) {
        if(alllist.lstResult[i].Company!= null ){
          if(!this.currentDatas.includes(alllist.lstResult[i].Company)){
              this.currentDatas.push(alllist.lstResult[i].Company)
          }
        }else if(alllist.lstResult[i].JobField != null) {
          if(!this.currentDatas.includes(alllist.lstResult[i].JobField)){
           this.currentDatas.push(alllist.lstResult[i].JobField)
          }
        }
        else if(alllist.lstResult[i].Name != null) {
          if(!this.currentDatas.includes(alllist.lstResult[i].Name)){
          this.currentDatas.push(alllist.lstResult[i].Name)
          }
        }
      }
      // if(!alllist.includes(this.contactForm.get('skills').value)){

      // }
      console.log(this.currentDatas);
      // this.currentDatas = res;
      // console.log(this.currentDatas.lstResult);

      // CORS ISSUE CODE
      let headers = new HttpHeaders()
      headers=headers.set('content-type','application/json');
      headers=headers.set('Access-Control-Allow-Origin', '*');
      console.log(event.target.value);
  
      return this.http.post('http://api.ms-employment.digisummits.com/Job/ForSearchValidation', event.target.value,{'headers':headers})
    })
  }
  industrySearch(event) {
    this.searchText=event.target.value
  }
  industrySearchC(event) {
    this.searchTextC=event.target.value
  }

  getLocation() {
    console.log("getLocation Called");
    var bdcApi = "https://api.bigdatacloud.net/data/reverse-geocode-client"
  
    navigator.geolocation.getCurrentPosition(
        (position) => {
            bdcApi = bdcApi
                + "?latitude=" + position.coords.latitude
                + "&longitude=" + position.coords.longitude
                + "&localityLanguage=en";
            this.getApi(bdcApi);
  
        },
        (err) => { this.getApi(bdcApi); },
        {
            enableHighAccuracy: true,
            timeout: 5000,
            maximumAge: 0
        });
  }
  
  getApi(bdcApi) {
    $('.preloaderSecond').css('display','block')
    this.http.get( bdcApi).subscribe((res)=>{
      var d  = Object(res) ;
      console.log(res);
      console.log(d.city);
      this.contactForm.controls.location.setValue(d.city);
      $('.preloaderSecond').hide()

      // console.log(res);
    })
    // Http.open("GET", bdcApi);
    // Http.send();
    // Http.onreadystatechange = function () {
    //     if (this.readyState == 4 && this.status == 200) {
    //         result.innerHTML = this.responseText;
    //     }
    // };
  }

  // industrySearch(event) {
  //   this.LkupJobFieldExtra = this.LkupIndustry
  //   console.log(event.target.value);
  //   // console.log(this.LkupJobFieldExtra)
  //   this.LkupJobFieldExtra.filter(item => {
  //     if(item.Name === event.target.value) {
  //       return item
  //     }
  //   })
  // }

  removeFilter() {
  // this.ngOnInit();
  $('input').parents('mat-checkbox').removeClass('mat-checkbox-checked')
  $('input').parents('mat-checkbox').addClass('cdk-focused cdk-program-focused')
  $('input').parents('mat-checkbox').attr('aria-checked="false"')
  $("input:checkbox:not(:checked)").prop('ariaChecked', false)
  $("input:checkbox:not(:checked)").prop('checked', false)
  // $("input:checkbox:not(:checked)").click()


  
  this.myCheckboxes.checked = false;
  this.req = {};    
  console.log(this.req)
  if(this.contactForm.get('location').value != '') {
    this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
  }
  if(this.contactForm.get('skills').value != '') {
    this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
  }
  console.log(this.arrayCheck)
  // let tryCheck = document.querySelector('.xyz')
  // tryCheck.forEach(i => {
  //   console.log(i)
  //   // console.log(i.target.checked)
  // })
  // this.contactForm.controls.skills.setValue('');
  // this.contactForm.controls.location.setValue('');
  this.req = {
    CompanyId:[],
    ExperienceId:[],
    IndustryId:[],
    JobFieldId:[],
    Qualificationid:[],
    filterText:'',
    cityId:''
  };
  this.arrayCheck = [];
  this.arrayCheck1 = [];
  this.arrayCheck2 = [];
  this.arrayCheck3 = [];
  this.arrayCheck4 = [];
  // this.Refbutton=false
  // this.searchComp=[]



  // this.searchComp= this.searchComp
  // this.LkupJobField= this.LkupJobField
  // this.LkupIndustry=this.LkupIndustry
  // this.LkupExperience=this.LkupExperience
  // this.LkupQualification=this.LkupQualification
  // $('.preloaderSecond').css('display','block')
  //   console.log(this.req);
  //   let req =  {CompanyId:''}
  //   let currentData =Object();
  //   let url = Constants.apiRoot+ "/Job/SearchCompany";
  //   this.http.post( url,this.req).subscribe((res)=>{
      this.currentData = this.currentDataOG = this.allDataBackup;
      console.log(this.currentData);
      this.allData=this.allDataBackup
      this.allDataBackup=this.allDataBackup
      this.onsubCountfilter(this.allDataBackup)
      this.setPage(1);
      
    //   $('.preloaderSecond').hide()
    //   // console.log(res);
    // })
  }

  skillset(i,event) {
    // console.log(i)
    console.log(event.target.innerHTML)
    this.contactForm.controls.skills.setValue(this.htmlDecode(event.target.innerHTML))
    this.contactForm.controls.location.setValue('')
    let data =  event.target.innerHTML
    this.request = {}
    console.log(data);
    this.onSubmit(data)
  }

  locationset(i,event) {
    // console.log(i)
    console.log(event.target.innerHTML)
    this.contactForm.controls.location.setValue(this.htmlDecode(event.target.innerHTML))
    this.contactForm.controls.skills.setValue('')
    let data =  event.target.innerHTML
    this.request = {}
    console.log(data);
    this.onSubmit(data)
  }

  company() {

  }


  // FOR LOOPS FOR API FILTER
  more3(event) {
    this.hideCheck3=!this.hideCheck3
    console.log(this.LkupExperience)
    this.LkupExperienceExtra = this.LkupExperience;
    this.less3=0;
    this.limts3+=this.LkupExperience.length
    
  }
  lessFun3() {
    this.hideCheck3=!this.hideCheck3
    console.log(this.LkupExperience)
    this.LkupExperienceExtra = this.LkupExperience;
    this.less3 = 0;
    this.limts3-=this.LkupExperience.length;
  }

  more2(event) {
    this.hideCheck2=!this.hideCheck2
    console.log(this.LkupIndustry)
    this.LkupIndustryExtra = this.LkupIndustry;
    this.less2=0;
    this.limts2+=this.LkupIndustry.length
    
  }
  lessFun2() {
    this.hideCheck2=!this.hideCheck2
    console.log(this.LkupIndustry)
    this.LkupIndustryExtra = this.LkupIndustry;
    this.less2 = 0;
    this.limts2-=this.LkupIndustry.length;
  }

  more1(event) {
    this.hideCheck1=!this.hideCheck1
    console.log(this.LkupJobField)
    this.LkupJobFieldExtra = this.LkupJobField;
    this.less1=0;
    this.limts1+=this.LkupJobField.length
    
  }
  lessFun1() {
    this.hideCheck1=!this.hideCheck1
    console.log(this.LkupJobField)
    this.LkupJobFieldExtra = this.LkupJobField;
    this.less1 = 0;
    this.limts1-=this.LkupJobField.length;
  }


  filterCheck(i,item) {
    // console.log(i)
    // console.log(item)
    // console.log(this.arrayCheck)
    if(this.arrayCheck.includes(item)){
      $('[value="'+item.toString()+'"]').parents('mat-checkbox').removeClass('mat-checkbox-checked')
      $('[value="'+item.toString()+'"]').prop('ariaChecked', false)
      $('[value="'+item.toString()+'"]').prop('checked', false)
    }
    this.arrayCheck.splice(i,1)
    // console.log(this.arrayCheck)
    this.req.JobFieldId =  this.arrayCheck
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
      }
    if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    $('[value="'+item.toString()+'"]').click()
    // console.log(this.req);
    // $('.preloaderSecond').css('display','block')
    // let url = Constants.apiRoot+ "/Job/Search";
    // this.http.post( url,this.req).subscribe((res)=>{
    //   this.currentData = res;
    //   console.log(this.currentData);
    //   this.objectnull(this.req)
    //   this.setPage(1);
    //   $('.preloaderSecond').hide()
    // })
  }
  filterCheck1(i,item) {
    if(this.arrayCheck1.includes(item)){
      $('[value="'+item.toString()+'"]').parents('mat-checkbox').removeClass('mat-checkbox-checked')
      $('[value="'+item.toString()+'"]').prop('ariaChecked', false)
      $('[value="'+item.toString()+'"]').prop('checked', false)
     
    }
    this.arrayCheck1.splice(i,1)
    this.req.IndustryId =  this.arrayCheck1
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
    }
    if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    $('[value="'+item.toString()+'"]').click()
  //   $('.preloaderSecond').css('display','block')
  // console.log(this.req);
  // let url = Constants.apiRoot+ "/Job/Search";
  //   this.http.post( url,this.req).subscribe((res)=>{
  //     this.currentData = res;
  //     this.objectnull(this.req)
  //     this.setPage(1);
  //     $('.preloaderSecond').hide()
  //     console.log(this.currentData);
  //     // console.log(res);
  //   })
  }
  filterCheck2(i,item) {
    if(this.arrayCheck2.includes(item)){
      $('[value="'+item.toString()+'"]').parents('mat-checkbox').removeClass('mat-checkbox-checked')
      $('[value="'+item.toString()+'"]').prop('ariaChecked', false)
      $('[value="'+item.toString()+'"]').prop('checked', false)
      
    }
    this.arrayCheck2.splice(i,1)
    this.req.ExperienceId = this.arrayCheck2
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
      }
      if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
      }
      $('[value="'+item.toString()+'"]').click()
      // console.log(this.req);
      // $('.preloaderSecond').css('display','block')
      // let url = Constants.apiRoot+ "/Job/Search";
      //   this.http.post( url,this.req).subscribe((res)=>{
      //     this.currentData = res;
      //     console.log(this.currentData);
      //     this.objectnull(this.req)
      //     this.setPage(1);
      //     $('.preloaderSecond').hide()
      //     // console.log(res);
      //   })
  }
  filterCheck3(i,item) {
    if(this.arrayCheck3.includes(item)){
      $('[value="'+item.toString()+'"]').parents('mat-checkbox').removeClass('mat-checkbox-checked')
      $('[value="'+item.toString()+'"]').prop('ariaChecked', false)
      $('[value="'+item.toString()+'"]').prop('checked', false)
     
    }
    this.arrayCheck3.splice(i,1)
    this.req.Qualificationid = this.arrayCheck3
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
    // this.req.cityId = this.sampleInput
      }
      if(this.contactForm.get('skills').value != '') {
      this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
      }
      $('[value="'+item.toString()+'"]').click()
    //   $('.preloaderSecond').css('display','block')
    // console.log(this.req);
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe((res)=>{
    //     this.currentData = res;
    //     console.log(this.currentData);
    //     this.objectnull(this.req)
    //     this.setPage(1);
    //     $('.preloaderSecond').hide()
    //     // console.log(res);
    //   })
  }
  filterCheck4(i,item){
    console.log(item);
    
    if(this.arrayCheck4.includes(item)){
      $('[value="'+item.toString()+'"]').parents('mat-checkbox').removeClass('mat-checkbox-checked')
      $('[value="'+item.toString()+'"]').prop('ariaChecked', false)
      $('[value="'+item.toString()+'"]').prop('checked', false)
     
      // $('[value="'+item.toString()+'"]').change()
    }
    this.arrayCheck4.splice(i,1)
    this.req.filterText = ''
    if(this.contactForm.get('location').value != '') {
      this.req.cityId = this.contactForm.get('location').value.split(',')
      }
    if(this.contactForm.get('skills').value != '') {
    this.req.filterText = this.contactForm.get('skills').value.toLowerCase()
    }
    $('[value="'+item.toString()+'"]').click()
   
    // $('.preloaderSecond').css('display','block')
    // console.log(this.req);
    // let url = Constants.apiRoot+ "/Job/Search";
    //   this.http.post( url,this.req).subscribe((res)=>{
    //     this.currentData = res;
    //     console.log(this.currentData);
    //     this.objectnull(this.req)
    //     this.setPage(1);
    //     $('.preloaderSecond').hide()
    //     // console.log(res);
    //   })
  }


  showList(){
    if(this.isReadMore){
      this.SCLTH=this.searchComp.length
    }else{
      this.SCLTH=5;
    }
    this.isReadMore = !this.isReadMore
  }
  
  // filtersearch(search:any) { 
  // }
  
  htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
  }

  setPage(page: number) {
    if(this.currentData.totalCount==0){
      this.pagedItems=[]
    }else{
      if (page < 1 || page >this.currentData.totalCount) {
        return;
    }

    // get pager object from service
    this.pager = this.PagerService.getPager(this.currentData.lstResult.length, page);

    // get current page of items
    this.pagedItems = this.currentData.lstResult.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }

}

objectnull(Obj){
  let  status=true
  if(Obj.CompanyId.length>0){
    status=false
  }
  if(Obj.ExperienceId.length>0){
    status=false
  }
  if(Obj.IndustryId.length>0){
    status=false
  }
  if(Obj.Qualificationid.length>0){
    status=false
  }
  if(Obj.JobFieldId.length>0){
    status=false
  }
  // if(Obj.filterText.length>0){
  //   status=false
  // }
  // if(Obj.cityId.length>0){
  //   status=false
  // }
  console.log(status);
  
  if(status){
    // $('.preloaderSecond').css('display','block')
    // let url = Constants.apiRoot+ "/Job/SearchCompany";
    // this.http.post( url,Object({CompanyId:''})).subscribe((res)=>{
      this.currentData = this.allDataBackup;
      // this.allData = this.allDataBackup;
      // console.log(this.currentData);
      // console.log(this.allDataBackup);
      // this.setPage(1);
      $('.preloaderSecond').hide()
      // console.log(res);
    // })
  }
 
}
getref(){
  $('input').parents('mat-checkbox').removeClass('mat-checkbox-checked')
  $('input').parents('mat-checkbox').addClass('cdk-focused cdk-program-focused')
  $('input').parents('mat-checkbox').attr('aria-checked="false"')
  this.arrayCheck = [];
  this.arrayCheck1 = [];
  this.arrayCheck2 = [];
  this.arrayCheck3 = [];
  this.arrayCheck4 = [];
  this.contactForm.controls.skills.setValue('');
  this.contactForm.controls.location.setValue('');
  this.req= {
    CompanyId:[],
    ExperienceId:[],
    IndustryId:[],
    JobFieldId:[],
    Qualificationid:[],
    filterText:'',
    cityId:''
  };
  this.Refbutton=false
  // this.searchComp=[]
    console.log(this.req);
    let req =  {CompanyId:''}
    let currentData =Object();
    $('.preloaderSecond').css('display','block')
    let url = Constants.apiRoot+ "/Job/SearchCompany";
    this.http.post( url,this.req).subscribe((res)=>{
      this.currentData = this.currentDataOG = res;
      this.allData=res
      this.allDataBackup=res
      console.log(this.currentData);
      this.onsubCountfilter(res)
      this.setPage(1);
      $('.preloaderSecond').hide()
      // console.log(res);
    })
}

filte(hf:any){
  var di =Object()
  if(hf.CompanyId.length>0){
console.log(this.arrayCheck4);

    // console.log( applyFilter(this.allData,{Company:hf.CompanyId}));
    for (let i = 0; i < this.arrayCheck4.length; i++) {
    
      this.arrayCheck4.push()
      
    }

    di.Company=hf.CompanyId
     
   }
   if(hf.ExperienceId.length>0){
    di.Experience=hf.ExperienceId
   
   }
   if(hf.IndustryId.length>0){
    di.Industry=hf.IndustryId
   }
   if(hf.JobFieldId.length>0){
    di.JobField=hf.JobFieldId
   }
   if(hf.Qualificationid.length>0){
    di.Qualification=hf.Qualificationid
   }
   console.log(di); 

  this.currentData={lstResult:applyFilter(this.allData,di)}
  this.currentData.totalCount=applyFilter(this.allData,di).length
  console.log( this.currentData);
  
  this.objectnull(hf)
  this.setPage(1)

}
fildsubmintfilte(hf:any){

 
  var di =Object()
  if(hf.CompanyId.length>0){
    di.Company=hf.CompanyId   
   }
   if(hf.ExperienceId.length>0){
    di.Experience=hf.ExperienceId
   
   }
   if(hf.IndustryId.length>0){
    di.Industry=hf.IndustryId
   }
   if(hf.JobFieldId.length>0){
    di.JobField=hf.JobFieldId
   }
   if(hf.Qualificationid.length>0){
    di.Qualification=hf.Qualificationid
   }
   console.log(di); 

  this.currentData={lstResult:applyFilter(this.allData,di)}
  this.currentData.totalCount=applyFilter(this.allData,di).length
  console.log( this.currentData);
  setTimeout(() => {
    $('.preloaderSecond').hide()
  }, 1000);
  this.objectnull(hf)
  this.setPage(1)
 

}
filtea(hf:any){
  // console.log(hf);
  // console.log(this.currentData);
  
 const dd:any=[];
  if(hf.CompanyId.length>0){

   console.log( applyFilter(this.allData,{Company:hf.CompanyId}));
   
      hf.CompanyId.forEach(dat => {
        dd.splice(0,0,...Object.assign(this.findata(dat,this.allData)))
    });
  }
  if(hf.ExperienceId.length>0){
    
    hf.ExperienceId.forEach(dat => {
      dd.splice(0,0,...Object.assign(this.findata(dat,this.allData)))
    });
  }
  if(hf.IndustryId.length>0){
    hf.IndustryId.forEach(dat => {
      dd.splice(0,0,...Object.assign(this.findata(dat,this.allData)))
    });
  }
  if(hf.JobFieldId.length>0){
    hf.JobFieldId.forEach(dat => {
      dd.splice(0,0,...Object.assign(this.findata(dat,this.allData)))
    });
  }
  if(hf.Qualificationid.length>0){
    hf.Qualificationid.forEach(dat => {
      dd.splice(0,0,...Object.assign(this.findata(dat,this.allData)))
    });
  }
  console.log(dd); 
  var data=dd
  this.currentData={lstResult:data}
  this.currentData.totalCount=dd.length
  console.log( this.currentData);
  
  this.objectnull(hf)
  this.setPage(1)
  
  // this.currentData
  

}

findata(d:any,Datafit:any){
  var Datafits=this.allDataBackup;
  return Datafits.lstResult.filter(x => {
    return Object.keys(x).some(key => {
      var Industry=x.Industry==null?'':x.Industry
      if(x.Company.toLowerCase()==d.toLowerCase()){
        return String(x[key]).toLowerCase().includes(d.toLowerCase());
      }
      if(x.Experience.toLowerCase()==d.toLowerCase()){
        return String(x[key]).toLowerCase().includes(d.toLowerCase());
      }
      if(Industry.toLowerCase()==d.toLowerCase()){
        return String(x[key]).toLowerCase().includes(d.toLowerCase());
      }
      if(x.JobField.toLowerCase()==d.toLowerCase()){
        return String(x[key]).toLowerCase().includes(d.toLowerCase());
      }
      if(x.Qualification.toLowerCase()==d.toLowerCase()){
        return String(x[key]).toLowerCase().includes(d.toLowerCase());
      }
  });
});
}



filterUsers(users:any, filter:any) {
  var result = [];
  
  for (var i=0;i<users.lstResult.length;i++){
      for (var prop in filter) {
        var f=0;
          if (users.lstResult[i][prop] == filter[prop][f]) {
              result.push(users.lstResult[i]);
              f++
          }
      }
  }
  return result;
}

}

