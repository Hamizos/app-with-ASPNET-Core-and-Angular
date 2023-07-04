import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-meber-detail',
  templateUrl: './meber-detail.component.html',
  styleUrls: ['./meber-detail.component.css']
})
export class MeberDetailComponent implements OnInit {
  member: Member;

 
  constructor(private memberService: MembersService, private route: ActivatedRoute) {
  
    
  }

  ngOnInit(): void {
    const name = this.route.snapshot.paramMap.get('name');

    if (name !== null) {
      this.memberService.getMember(name).subscribe(member => {
        // Handle the returned member data
        this.member = member;
      });
  }
}

}
