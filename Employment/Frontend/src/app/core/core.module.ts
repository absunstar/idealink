import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './auth-interceptor.service';
import { AuthService } from './auth-service.component';
import { AccountService } from './account.service';
import { ProjectService } from './project.service';
import { AdminRouteGuard } from './admin-route-guard';
import { FilterPipe } from './pipe/filter.pipe';
import { SearchfilterPipe } from './pipe/searchfilter.pipe';

@NgModule({
    imports: [],
    exports: [
        FilterPipe
    ],
    declarations: [
        FilterPipe,
        SearchfilterPipe
    ],
    providers: [
        AuthService,
        AccountService,
        ProjectService,
        AdminRouteGuard,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true }
    ],
})
export class CoreModule { }
