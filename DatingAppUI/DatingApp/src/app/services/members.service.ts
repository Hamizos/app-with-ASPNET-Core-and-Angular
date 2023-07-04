import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Member } from '../models/member';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseApiUrl: string = 'https://localhost:7245/';

  constructor(private http: HttpClient) { }

  getMembers(): Observable<Member[]>{
    return  this.http.get<Member[]>(this.baseApiUrl + 'api/users')
  }

  getMember(name: string): Observable<Member>{
    return this.http.get<Member>(this.baseApiUrl + 'api/users/' + name);
  }

  updateMembers(member: Member){
    return this.http.put(this.baseApiUrl + 'api/users', member)
  } 

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseApiUrl + 'api/users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseApiUrl + 'api/users/delete-photo/' + photoId)
  }

}
