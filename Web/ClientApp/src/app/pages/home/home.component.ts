import { Component, OnInit } from '@angular/core';

/**
 * Home page component.
 */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    constructor() {
        //translate.addLangs(['en', 'fr']);
        //translate.setDefaultLang('en');

        //const browserLang = translate.getBrowserLang();
        //translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');
    }

  ngOnInit(): void {
  }

}
