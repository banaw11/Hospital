import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NbMenuItem, NbMenuService, NbSidebarService } from '@nebular/theme';
import { delay, filter, map } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  menuItems: NbMenuItem[] = [
    {
      title:'',
      icon: "home",
      link: "/home",
      home: true,
      
    },
    {
      title:"Pracownicy",
      icon: "people-outline",
      link: "/home/employees"
    },
    {
      title: "Dyżury",
      icon:"calendar-outline",
      link: "/home/schedule"
    }
  ]

  constructor(public sidebarService: NbSidebarService, private menuService: NbMenuService, private router: Router, public accountService: AccountService) { }

  ngOnInit(): void {
    this.onMenuClick();
  }

  onMenuClick(){
    this.menuService.onItemClick().pipe(
      map(({item: {link}}) => link),
    ).subscribe(link => {
      if(link)
      {
        this.router.navigateByUrl(link);
      }
    })
  }

}
