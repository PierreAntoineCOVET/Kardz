import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { CoincheComponent } from '../games/coinche/coinche.component';
import { GamesModule } from '../games/games.module';
import { TranslateModule } from '@ngx-translate/core';

/**
 * Module registering all the pages components.
 */
@NgModule({
    declarations: [HomeComponent],
    imports: [
        CommonModule,
        GamesModule,
        TranslateModule
    ],
    exports: [
        HomeComponent
    ],
    entryComponents: [
        CoincheComponent
    ]
})
export class PagesModule { }
