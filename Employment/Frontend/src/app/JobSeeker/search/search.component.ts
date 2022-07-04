import { Component, OnInit } from '@angular/core';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { cGenericIdNameItem, cGenericSubItem } from 'src/app/interface/Response/GenericIdName.class';
import { ServiceJobSeeker } from 'src/app/services/job-seeker.service';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { ServiceQualification } from 'src/app/services/qualification.service';
import { ServiceYearsOfExperience } from 'src/app/services/years-of-experience.service';
import { ServiceFavourite } from 'src/app/services/favourite.service';
import { ServiceLanguages } from 'src/app/services/languages.service';
import { ModelJobSeekerSearch } from 'src/app/interface/Model/ModelJobSeekerSearch.class';
import { Page } from 'src/app/common/pagination/page';
import { cJobSeekerList, cJobSeekerItem } from 'src/app/interface/Response/JobSeeker.class';
import { CustomPaginationService } from 'src/app/common/pagination/services/custom-pagination.service';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/constants';
import { ServiceJob } from 'src/app/services/job.service';
import { ServiceCountry } from 'src/app/services/country.service';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { PagerService } from 'src/app/Employer/pager.service';
declare var $: any;
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent extends baseComponent implements OnInit {

  LkupQualification: cGenericSubItem[];
  LkupExperience: cGenericSubItem[];
  LkupLanguage: cGenericSubItem[];
  LkupCountry: cGenericIdNameItem[];
  LkupCity: cGenericSubItem[];
  currentDatas: any;
  modelSearch: ModelJobSeekerSearch = new ModelJobSeekerSearch();
  lstResult: cJobSeekerList;
  page: Page<cJobSeekerItem> = new Page();
  searchData:any=[]


  filtertxt: string = '';
  arrayCheck: any=[];
  arrayCheck1: any=[];
  arrayCheck2: any=[];
  arrayCheck3: any=[];
  arrayCheck4: any=[];
  arrayCheck5: any=[];
  modelSearchR:any={}
  onlySkillHTM:any=this.cookies.get('setrecent') ? JSON.parse(this.cookies.get('setrecent')) : []
  onlySkill:any=this.cookies.get('setrecent') ? JSON.parse(this.cookies.get('setrecent')) : []



  SearchbyGender:any=[
    { Name: "Male",  _id: "1", IsActive: true },
    { Name: "Female", _id: "2", IsActive: true }
  ];
  allItems: any[];

  // pager object
  pager: any = {};

  // paged items
  pagedItems: any=[];
  constructor(
    private BLCountry: ServiceCountry,
    private BLJobSeeker: ServiceJobSeeker,
    private BLService: ServiceJob,
    BLServiceShowMessage: ServiceShowMessage,
    BLJobSeekerLoginUser: ServiceLoginUser,BLTranslate: TranslateService,
    private BLQualification: ServiceQualification,
    private BLExperience: ServiceYearsOfExperience,
    private BLFavourite: ServiceFavourite,
    private cookies :CookieService,
    private paginationService: CustomPaginationService,
    private http: HttpClient,
    private PagerService :PagerService,
    private BLLanguages: ServiceLanguages) {
    super(BLServiceShowMessage, BLJobSeekerLoginUser, BLTranslate)
  }
  ngOnInit(): void {
  
    console.log(this.onlySkillHTM);
    let privT =  this.cookies.get('priST')
    console.log(this.onlySkill);
    if(this.onlySkill && privT=='true'){
      this.cookies.set('priST',JSON.stringify(false))
      if(this.onlySkill.length==0){
        var a=0
      }else{
        var a=this.onlySkill.length-1
      }
      this.modelSearch.filterText=this.onlySkill[a]
      this.aipfiltercall()
    }else{
      this.loadData();
    }
    // let req=Object({filterText:this.filtertxt})
    // this.BLService.Search(req).subscribe({
    //   next: response => {
    //     this.searchData= response.lstResult
    //   }  
    // })
    console.log(this.SearchbyGender);
    
    this.BLCountry.getListActive().subscribe({
      next: lst => {
        this.LkupCountry = lst;
        // this.onCountrySelect('','');
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
    // this.lstResult=Object({totalCount:1 })
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
    this.BLJobSeeker.Search(this.modelSearch).subscribe({
      next: lst => { 
        this.lstResult = lst;
        this.page.pageable.pageSize = lst.pageSize;
        this.page.totalElements = lst.totalCount;
        this.page.content = lst.lstResult;
        this.setPage(1)
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
  // onCountrySelect() {
  //   this.LkupCity = [];

  //   var sub = this.LkupCountry?.find(x => x._id == this.modelSearch.CountryId)?.subItems;
  //   if (!sub || sub.length == 0)
  //     return;

  //   this.LkupCity = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
  //   this.FilterData();
  // }
  submit() { }
  onCountrySelect(i,item) {
    console.log(item);
    
    item.IsActive=!item.IsActive
    this.LkupCity = [];
    if(!item.IsActive){
      var che= this.arrayCheck4.some(function(el) {
        return el.Name === item.Name;
      }); 
      
      if(!che){
        // this.arrayCheck4.push(item)
        this.arrayCheck4=[item]
        this.modelSearchR.CountryId=item._id

      }  
      $('html, body').animate({scrollTop:800}, 'slow');
      console.log(this.arrayCheck4);
        var sub = this.LkupCountry?.find(x => x._id == item._id)?.subItems;
        if (!sub || sub.length == 0)
          return;

        this.LkupCity = sub.filter(y => y.IsActive == true).sort((a, b) => a.Name.localeCompare(b.Name));
        
     
    }else{
      const index = this.arrayCheck4.indexOf(item);
      if (index > -1) {
        this.arrayCheck4.splice(index, 1);
      }
      this.arrayCheck5=[]
      this.modelSearchR.CountryId=''
      this.modelSearchR.CityId='';
    }
    console.log(this.arrayCheck4);
    
    this.aipfiltercall()
    
  }

  FilterData1(e,n:cGenericSubItem){
    n.IsActive=!n.IsActive
    Object.assign(this.LkupExperience);
    if(!n.IsActive){
     var che= this.arrayCheck.some(function(el) {
        return el.Name === n.Name;
      }); 
      if(!che){
        this.arrayCheck.push(n)
      }
    }else{
      const index = this.arrayCheck.indexOf(n);
      if (index > -1) {
        this.arrayCheck.splice(index, 1);
      }
    }
    // this.modelSearch.ExperienceId=this.filterreq(this.arrayCheck)
    // console.log(this.modelSearch);
    this.aipfiltercall()

  }
  FilterData2(e,n:cGenericSubItem){
    n.IsActive=!n.IsActive
    Object.assign(this.LkupExperience);
    if(!n.IsActive){
     var che= this.arrayCheck1.some(function(el) {
        return el.Name === n.Name;
      }); 
      if(!che){
        this.arrayCheck1.push(n)
      }
    }else{
      const index = this.arrayCheck1.indexOf(n);
      if (index > -1) {
        this.arrayCheck1.splice(index, 1);
      }
    }
    // this.modelSearch.ExperienceId=this.filterreq(this.arrayCheck)
    // console.log(this.modelSearch);
    this.aipfiltercall()

  }
  FilterData3(e,n:cGenericSubItem){
    n.IsActive=!n.IsActive
    Object.assign(this.LkupExperience);
    if(!n.IsActive){
     var che= this.arrayCheck2.some(function(el) {
        return el.Name === n.Name;
      }); 
      if(!che){
        this.arrayCheck2.push(n)
      }
    }else{
      const index = this.arrayCheck2.indexOf(n);
      if (index > -1) {
        this.arrayCheck2.splice(index, 1);
      }
    }
    // this.modelSearch.ExperienceId=this.filterreq(this.arrayCheck)
    // console.log(this.modelSearch);
    this.aipfiltercall()

  }
  FilterData4(e,n:cGenericSubItem){
    n.IsActive=!n.IsActive
    Object.assign(this.LkupExperience);
    if(!n.IsActive){
     var che= this.arrayCheck3.some(function(el) {
        return el.Name === n.Name;
      }); 
      if(!che){
        this.arrayCheck3.push(n)
      }
    }else{
      const index = this.arrayCheck3.indexOf(n);
      if (index > -1) {
        this.arrayCheck3.splice(index, 1);
      }
    }
    // this.modelSearch.ExperienceId=this.filterreq(this.arrayCheck)
    // console.log(this.modelSearch);
    this.aipfiltercall()

  }
  FilterData5(e,n:cGenericSubItem){

    
    if(n.IsActive){
      this.LkupCity.forEach(element => {
        element.IsActive = true;
      });
    }
    n.IsActive=!n.IsActive

    Object.assign(this.LkupCity);
    if(!n.IsActive){
     var che= this.arrayCheck5.some(function(el) {
        return el.Name === n.Name;
      }); 
      if(!che){
        // this.arrayCheck5.push(n)
        this.arrayCheck5=[n]
        this.modelSearchR.CityId=n._id
      }
      
    }else{
      const index = this.arrayCheck5.indexOf(n);
      if (index > -1) {
        this.arrayCheck5.splice(index, 1);
      }
      this.modelSearchR.CityId=''
      this.arrayCheck5=[]

    }
    // this.modelSearch.ExperienceId=this.filterreq(this.arrayCheck)
    // console.log(this.modelSearch);
    this.aipfiltercall()

  }

  filterCheck(i,item:cGenericSubItem) {
    item.IsActive=!item.IsActive
    this.arrayCheck.splice(i,1)
    this.aipfiltercall()
  }
  filterCheck2(i,item:cGenericSubItem) {
    item.IsActive=!item.IsActive
    this.arrayCheck1.splice(i,1)
    this.aipfiltercall()
  }
  filterCheck3(i,item:cGenericSubItem) {
    item.IsActive=!item.IsActive
    this.arrayCheck2.splice(i,1)
    this.aipfiltercall()
  }
  filterCheck4(i,item:cGenericSubItem) {
    item.IsActive=!item.IsActive
    this.arrayCheck3.splice(i,1)
    this.aipfiltercall()
  }
  filterCheck5(i,item) {
    item.IsActive=!item.IsActive
    this.arrayCheck4.splice(i,1)
    this.modelSearchR.CountryId=''
    this.aipfiltercall()
  }
  filterCheck6(i,item) {
    item.IsActive=!item.IsActive
    this.arrayCheck5.splice(i,1)
    this.modelSearchR.CityId=''
    this.aipfiltercall()
  }

  aipfiltercall(){
    this.modelSearchR.ExperienceId=this.filterreq(this.arrayCheck)
    this.modelSearchR.GenderId=this.filterreq(this.arrayCheck1)
    this.modelSearchR.LanguageId=this.filterreq(this.arrayCheck2)
    this.modelSearchR.Qualificationid=this.filterreq(this.arrayCheck3)
    // this.modelSearchR.CountryId=this.filterreq(this.arrayCheck4)
    // this.modelSearchR.CityId=this.filterreq(this.arrayCheck5)
    this.modelSearchR.filterText = this.modelSearch.filterText!=''?this.modelSearch.filterText:'';
    this.modelSearchR.CurrentPage = this.page.pageable.pageCurrent;
    if(this.modelSearch.filterText!=''){
      if(!this.onlySkill.includes(this.modelSearch.filterText)){
        this.onlySkill.push(this.modelSearch.filterText)
      }
    }
  
    this.cookies.set('setrecent',JSON.stringify(this.onlySkill))
    console.log(this.modelSearchR);
    $('.preloaderSecond').show()
    setTimeout(() => {
      this.BLJobSeeker.Search(this.modelSearchR).subscribe({
        next: lst => { 
          this.lstResult = lst;
          this.page.pageable.pageSize = lst.pageSize;
          this.page.totalElements = lst.totalCount;
          this.page.content = lst.lstResult;
          this.setPage(1)
          $('.preloaderSecond').hide()
        },
        error: err => {
          $('.preloaderSecond').hide()
          this.message.Error(err);
          this.BLServiceShowMessage.sendMessage(this.message);

        }
      });
    }, 50);
   

  }
  filterreq(datareq){
    var dta=[]
    datareq.forEach(data => {
      dta.push(data._id)
    });
    return dta.length==0?[]:dta.map(v => v.toLowerCase())
  }
  skillset(e,it){
    this.modelSearch.filterText=it;
    this.aipfiltercall()

  }
  htmlDecode(input) {
    var doc = new DOMParser().parseFromString(input, "text/html");
    return doc.documentElement.textContent;
  }
  removeFilter(){
    this.arrayCheck=[];
    this.arrayCheck1=[];
    this.arrayCheck2=[];
    this.arrayCheck3=[];
    this.arrayCheck4=[];
    this.arrayCheck5=[];
    this.SearchbyGender={}
    this.LkupCity=[]
    this.modelSearch.filterText=''
    this.modelSearchR.CityId='';
    this.modelSearchR.CountryId='';
    this.modelSearchR.filterText='';
    this.allclearFilter()
    this.aipfiltercall()
  }

  allclearFilter(){
    this.SearchbyGender=[
      { Name: "Male",  _id: "1", IsActive: true },
      { Name: "Female", _id: "2", IsActive: true }
    ];
    this.BLCountry.getListActive().subscribe({
      next: lst => {
        this.LkupCountry = lst;
        // this.onCountrySelect('','');
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
  }

  search(event) {
    console.log(event.target.value);
    event.target.value
    // let url = "https://employmentapi.idealake.com/Job/ForSearchValidation"
    let url = Constants.apiRoot+ "Job/ForSearchValidation"
    this.http.post(url ,{
      filterTextValidation : event.target.value
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
      // console.log(this.currentDatas);
      // this.currentDatas = res;
      // console.log(this.currentDatas.lstResult);

   })
  }

  setPage(page: number) {
    if(this.lstResult.totalCount==0){
      this.pagedItems=[]
    }
    if (page < 1 || page >this.lstResult.totalCount) {
        return;
    }

    // get pager object from service
    this.pager = this.PagerService.getPager(this.lstResult.lstResult.length, page);

    // get current page of items
    this.pagedItems = this.lstResult.lstResult.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }
}
