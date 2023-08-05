import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment'
import { UserForLogin, UserForRegister } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }

  authUser(user: UserForLogin){
    console.log(this.baseUrl);
    return this.http.post<UserForLogin>(this.baseUrl + '/account/login', user);
    /* let UserArray = [];
    if(localStorage.getItem('Users')){
      UserArray = JSON.parse(localStorage.getItem('Users') || '{}');
    }
    return UserArray.find((p: { userName: any; password: any; }) => p.userName === user.userName && p.password === user.password); */
  }

  registerUser(user: UserForRegister){
    return this.http.post<UserForLogin>(this.baseUrl + '/account/register', user);
  }


}
