import { Component, OnInit } from '@angular/core';
import { NbSidebarService } from '@nebular/theme';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  dateTime : Date = new Date();
  constructor() {
    setInterval(() => {
      this.dateTime = new Date();
    },1);
   }

  ngOnInit(): void {
    
  }

}
