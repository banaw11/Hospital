import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbActionsModule, NbButtonModule, NbUserModule, NbContextMenuModule, NbMenuModule, NbCardModule, NbInputModule,
NbAlertModule, NbSidebarModule, NbIconModule, NbListModule, NbSelectModule, NbCheckboxModule, NbTabsetModule} from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './components/home/home.component';
import { NavComponent } from './modules/nav/nav.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { LoginComponent } from './modules/forms/login/login.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { IndexComponent } from './components/index/index.component';
import { EmployeComponent } from './components/employe/employe.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { ProfileComponent } from './components/profile/profile.component';
import { CallendarComponent } from './modules/callendar/callendar.component';
import { AdministrationEmployeesComponent } from './components/administration-employees/administration-employees.component';
import { AdministrationSchedulesComponent } from './components/administration-schedules/administration-schedules.component';
import { AdministrationAccountsComponent } from './components/administration-accounts/administration-accounts.component';
import { RegisterComponent } from './modules/forms/register/register.component';
import { AccountEditorComponent } from './modules/forms/account-editor/account-editor.component';
import { EmployeeSelectComponent } from './modules/employee-select/employee-select.component';
import { PasswordResetComponent } from './modules/forms/password-reset/password-reset.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    LoginComponent,
    IndexComponent,
    EmployeComponent,
    ScheduleComponent,
    ProfileComponent,
    CallendarComponent,
    AdministrationEmployeesComponent,
    AdministrationSchedulesComponent,
    AdministrationAccountsComponent,
    RegisterComponent,
    AccountEditorComponent,
    EmployeeSelectComponent,
    PasswordResetComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    FormsModule,
    NbThemeModule.forRoot({ name: 'corporate' }),
    NbLayoutModule,
    NbEvaIconsModule,
    NbActionsModule,
    NbButtonModule,
    NbUserModule,
    NbContextMenuModule,
    NbMenuModule.forRoot(),
    NbCardModule,
    NbInputModule,
    NbAlertModule,
    NbSidebarModule.forRoot(),
    NbIconModule,
    NbListModule,
    NbSelectModule,
    NbCheckboxModule,
    NbTabsetModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
