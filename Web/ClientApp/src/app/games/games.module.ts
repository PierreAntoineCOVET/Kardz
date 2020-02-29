import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoincheComponent } from './coinche/coinche.component';
import { HttpClientModule } from '@angular/common/http';

/**
 * Module for all the games logic.
 */
@NgModule({
    declarations: [CoincheComponent],
    imports: [
        CommonModule,
        HttpClientModule
    ],
    exports: [
        CoincheComponent
    ]
})
export class GamesModule { }
