import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './modules/forms/login/login.component';

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'home/login'},
  {path: 'home', component: HomeComponent,
  runGuardsAndResolvers : 'always',
   children: [
     {path: '', component: IndexComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
     {path: 'login', component: LoginComponent}
   ]},
  {path: '**', pathMatch: 'full' , redirectTo: 'home/login'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
