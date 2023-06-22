import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './shared/home/home.component';
import { MessagesComponent } from './components/messages/messages.component';
import { ListsComponent } from './components/lists/lists.component';
import { authGuard } from './Gaurds/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'members',
    loadChildren: () =>
      import('./member/member.module').then((m) => m.MemberModule),
  },
  { path: 'messages', component: MessagesComponent, canActivate: [authGuard] },
  { path: 'lists', component: ListsComponent, canActivate: [authGuard] },
  // { path: '**', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
