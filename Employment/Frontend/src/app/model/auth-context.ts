import { UserProfile } from './user-profile';
import { SimpleClaim } from './simple-claim';

export class AuthContext {
  claims: SimpleClaim[];
  userProfile: UserProfile;
  get getUserProfile(){
    if(!!this.userProfile)
    {
      return this.userProfile;
    }
    return  null;
  }
  get userRole(){ 
    if(!!this.claims && !!this.claims.find(c => c.Type === 'role'))
    {
      return this.claims.find(c => c.Type === 'role').Value;
    }
    return  "";
  }
  get userName(){
    if(!!this.userProfile)
    {
      return this.userProfile.FullName;
    }
    return  "";
  }
  get userId(){
    if(!!this.claims && !!this.claims.find(c => c.Type === 'MDID'))
    {
      return this.claims.find(c => c.Type === 'MDID').Value;
    }
    return  "-1";
  }
  get isAdmin() { 
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'admin');
  }
  get isEmployer(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'employer');
  }
  get isJobSeeker(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'jobseeker');
  }
}