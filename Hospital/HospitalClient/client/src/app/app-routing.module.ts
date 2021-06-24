import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeComponent } from './components/employe/employe.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { AuthGuard } from './guards/auth.guard';
import { IndexComponent } from './components/index/index.component';
import { LoginComponent } from './modules/forms/login/login.component';
import { AdministrationEmployeesComponent } from './components/administration-employees/administration-employees.component';
import { AdminGuard } from './guards/admin.guard';
import { AdministrationSchedulesComponent } from './components/administration-schedules/administration-schedules.component';
import { AdministrationAccountsComponent } from './components/administration-accounts/administration-accounts.component';

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'home'},
  {path: 'home', component: HomeComponent,
  runGuardsAndResolvers : 'always',
   children: [
    {path: '', component: IndexComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
    {path: 'schedule', component: ScheduleComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
    {path: 'employees', component: EmployeComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
    {path: 'profile', component: ProfileComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
    {path: 'administration-employees', component: AdministrationEmployeesComponent, runGuardsAndResolvers: 'always', canActivate: [AdminGuard]},
    {path: 'administration-schedules', component: AdministrationSchedulesComponent, runGuardsAndResolvers: 'always', canActivate: [AdminGuard]},
    {path: 'administration-accounts', component: AdministrationAccountsComponent, runGuardsAndResolvers: 'always', canActivate: [AdminGuard]},
    {path: 'login', component: LoginComponent}
   ]},
  {path: '**', pathMatch: 'full' , redirectTo: 'home'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
