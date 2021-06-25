import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbMenuService } from '@nebular/theme';
import { filter, map } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  isLogged: boolean = false;
  items = [
    {title: 'Profil'},
    {title: 'Wyloguj'}
  ];
  userName?: string ;
  constructor(public accountService: AccountService, private menuService: NbMenuService, private router: Router) { 
    this.accountService.currentUser$.subscribe(user => {
      if(user){
        this.userName = user.name;
      }
    })
  }

  ngOnInit(): void {
    this.onMenuClick();
  }


  onMenuClick(){
    this.menuService.onItemClick().pipe(
      filter(({tag}) => tag === 'avatar-context'),
      map(({item: {title}}) => title),
    ).subscribe(title => {
      if(title === 'Wyloguj'){
        this.accountService.logOut();
        this.router.navigateByUrl('');
      }
      else if(title === 'Profil'){
        this.router.navigateByUrl('/home/profile');
      }
    })
  }

}
