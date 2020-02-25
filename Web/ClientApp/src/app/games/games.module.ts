import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoincheComponent } from './coinche/coinche.component';



@NgModule({
    declarations: [CoincheComponent],
    imports: [
        CommonModule
    ],
    exports: [
        CoincheComponent
    ]
})
export class GamesModule { }
