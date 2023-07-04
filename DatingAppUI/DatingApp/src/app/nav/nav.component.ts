import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Observable, observable } from 'rxjs';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { Member } from '../models/member';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}
  currentUser$: Observable<User | null>;

constructor(private accountService: AccountService, private router: Router) {
 
  
}
  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('members')
      },
      error: (response) => {
        console.log(response);
      }
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }


}
