/// <reference types="@angular/localize" />

import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient }    from '@angular/common/http';
import { provideRouter }        from '@angular/router';
import { importProvidersFrom }  from '@angular/core';
import { ReactiveFormsModule  } from '@angular/forms';

import { AppComponent } from './app/app.component';
import { appConfig }    from './app/app.config';
import { routes }       from './app/app.routes';


bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...(appConfig.providers ?? []),
    provideRouter(routes),
    provideHttpClient(),
    importProvidersFrom(ReactiveFormsModule),
  ]
})
.catch(err => console.error(err));
