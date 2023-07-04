import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { AccountService } from './account.service';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuardService {

  constructor(private route: Router, private accountService: AccountService) { }

  canActivate(): Observable<boolean>{
    return this.accountService.currentUser$.pipe(
      map(user => {
        if(user) return true;
        alert('You shall not pass!!');
        return false;
      })
    )
  }
}
