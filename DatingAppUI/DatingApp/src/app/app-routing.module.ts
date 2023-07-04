import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MeberDetailComponent } from './members/meber-detail/meber-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { GuardService } from './services/guard.service';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { canDeactivateGuard } from './services/can-deactivate.guard';


const routes: Routes = [

  {
    path: '',
    component: HomeComponent
  },
  {
    path:'',
    runGuardsAndResolvers: 'always',
    canActivate: [GuardService],
    children: [
      {
        path: 'members',
        component: MemberListComponent,
        
      },
      {
        path: 'members/:name',
        component: MeberDetailComponent
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        canDeactivate: [canDeactivateGuard]
      },
      {
        path: 'lists',
        component: ListsComponent
      },
      {
        path: 'messages',
        component: MessagesComponent
      },
    ]
  },
  {
    path: '**',
    component: HomeComponent, pathMatch: 'full'
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
