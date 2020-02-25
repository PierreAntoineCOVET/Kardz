import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { PagesModule } from './pages/pages.module';
import { HomeComponent } from './pages/home/home.component';

@NgModule({
    imports: [
        BrowserModule,
        AppRoutingModule,
        PagesModule
    ],
    providers: [],
    bootstrap: [HomeComponent]
})
export class AppModule { }
