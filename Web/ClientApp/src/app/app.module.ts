import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { LOCATION_INITIALIZED } from '@angular/common';
import { HttpClient } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { PagesModule } from './pages/pages.module';
import { HomeComponent } from './pages/home/home.component';

import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

/**
 * Init translator.
 * @param httpClient http client
 */
export function appInitializerFactory(translate: TranslateService, injector: Injector) {
    return () => new Promise<any>((resolve: any) => {
        const locationInitialized = injector.get(LOCATION_INITIALIZED, Promise.resolve(null));
        locationInitialized.then(() => {
            const browserLang = translate.getBrowserLang();
            const langToSet = browserLang.match(/en|fr/) ? browserLang : 'en';
            translate.use(langToSet)
                .subscribe({
                    next: () => {
                        console.info(`Successfully initialized '${langToSet}' language.'`);
                        console.log(translate.instant('game.lobby.title'));
                    },
                    error: (reason) => {
                        console.error(`Problem with '${langToSet}' language initialization :`);
                        console.error(reason);
                    },
                    complete: resolve(null)
                });
        });
    });
}

export function HttpLoaderFactory(httpClient: HttpClient) {
    return new TranslateHttpLoader(httpClient);
}

@NgModule({
    imports: [
        BrowserModule,
        AppRoutingModule,
        PagesModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        })
    ],
    providers: [
        {
            provide: APP_INITIALIZER,
            useFactory: appInitializerFactory,
            deps: [TranslateService, Injector],
            multi: true
        }
    ],
    bootstrap: [HomeComponent]
})
export class AppModule { }
