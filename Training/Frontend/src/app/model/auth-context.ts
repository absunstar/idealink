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
  get isAdmin() {
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'admin');
  }
  get isPartner(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'partner');
  }
  get isSubPartner(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'subpartner');
  }
  get isTrainer(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'trainer');
  }
  get isTrainee(){
    return !!this.claims && !!this.claims.find(c =>
      c.Type === 'role' && c.Value.toLowerCase() === 'trainee');
  }
}