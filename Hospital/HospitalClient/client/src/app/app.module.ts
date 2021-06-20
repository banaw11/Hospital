import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbActionsModule, NbButtonModule, NbUserModule, NbContextMenuModule, NbMenuModule, NbCardModule, NbInputModule,
NbAlertModule, NbSidebarModule, NbIconModule, NbListModule, NbSelectModule, NbCheckboxModule} from '@nebular/theme';
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

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    LoginComponent,
    IndexComponent,
    EmployeComponent,
    ScheduleComponent,
    ProfileComponent
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
    NbCheckboxModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
