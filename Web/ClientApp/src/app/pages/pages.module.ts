import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { CoincheComponent } from '../games/coinche/coinche.component';
import { GamesModule } from '../games/games.module';

/**
 * Module registering all the pages components.
 */
@NgModule({
    declarations: [HomeComponent],
    imports: [
        CommonModule,
        GamesModule,
    ],
    exports: [
        HomeComponent
    ]
})
export class PagesModule { }
