import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor, HttpResponse, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators'
import { ProgressSpinnerService } from '.';


@Injectable()
export class InterceptorService implements HttpInterceptor {
    constructor(private progressSpinnerService: ProgressSpinnerService) {
    }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.progressSpinnerService.setLoading(true, request.url);
        return next.handle(request)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    this.progressSpinnerService.setLoading(false, request.url);
                    let errorMsg = '';
                    if (error.error instanceof ErrorEvent) {
                        console.log('This is client side error');
                        errorMsg = `Error: ${error.error.message}`;
                    } else {
                        console.log('This is server side error');
                        errorMsg = `Error Code: ${error.status},  Message: ${error.message}`;
                    }
                    console.log(errorMsg);
                    return throwError(error.error);
                }))
            .pipe(
                map<any, HttpEvent<any>>((evt: HttpEvent<any>) => {
                    if (evt instanceof HttpResponse) {
                        this.progressSpinnerService.setLoading(false, request.url);
                    }
                    return evt;
                })
            );
    }
}