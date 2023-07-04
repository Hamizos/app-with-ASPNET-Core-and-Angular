import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseApiUrl: string = 'https://localhost:7245/';
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();



  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post<User>(this.baseApiUrl + 'api/account/login' , model).pipe(
      map((response: User) => {
        const user = response;
        if(user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  register(model: any){
    return this.http.post<User>(this.baseApiUrl + 'api/account/register' , model).pipe(
      map((response: User) => {
        const user = response;
        if(user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        } 
      })
    )
  }

  getUsers(){
    return this.http.get(this.baseApiUrl + 'api/users');
  }


  
}
  