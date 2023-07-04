import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User | null;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any){
    if(this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  
  constructor(private accountService: AccountService, private memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
  }
  ngOnInit(): void {

    if (this.user !== null) {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    
    });
  }
}

  updateMember(){
    this.memberService.updateMembers(this.member).subscribe(() => {
      this.editForm.reset(this.member);
    })
    
    
  }
}
