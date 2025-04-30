import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { errorInterceptor } from './config/http-interceptor';
import { JwtModule } from '@auth0/angular-jwt';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { authInterceptor } from './config/auth-interceptor';
import { TimeagoModule } from 'ngx-timeago';


registerLocaleData(en);


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideNzI18n(en_US),
    importProvidersFrom(FormsModule, TimeagoModule.forRoot()),
    provideAnimationsAsync(),
    provideCharts(withDefaultRegisterables()), // TODO: Consider including a minimal configuration
    provideHttpClient(
      withInterceptors([errorInterceptor, authInterceptor]),
      withInterceptorsFromDi()
    ),
    importProvidersFrom([
      JwtModule.forRoot({
        config: {},
    }),
    ])
  ]
};
