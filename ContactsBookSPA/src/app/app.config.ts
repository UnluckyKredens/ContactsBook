import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideToastr()
  ]
};
