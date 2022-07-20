import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable, of, retry, tap } from "rxjs";
import { ConfigService } from "../config/config.service";
import { ToastrService } from 'ngx-toastr';

class ApiResponse<T>{
    data: T;
    errors: any[];
    responseCode: any;
}

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
    providedIn: 'root'
})
export class HttpService {

    private sellerBaseUrl: string;
    private buyerBaseUrl: string;

    constructor(private httpclient: HttpClient, private configservice: ConfigService, private toastrService: ToastrService) {
        this.sellerBaseUrl = this.configservice.getApi("sellerBaseUrl");
        this.buyerBaseUrl = this.configservice.getApi("buyerBaseUrl");
    }

    private handleError<T>(operation = 'operation', result?: ApiResponse<T>) {
        return (apiResponse: ApiResponse<T>): Observable<T> => {

// if(apiResponse == null)

            // TODO: send the error to remote logging infrastructure
            console.error(apiResponse.errors.join(" , ")); // log to console instead

            // TODO: better job of transforming error for user consumption
            this.logger(`${operation} failed: ${apiResponse.errors.join(" , ")}`);

            this.toastrService.error(apiResponse.errors.join(" , "), "", {
                timeOut: 3000,
                closeButton: false
            });

            // Let the app keep running by returning an empty result.
            return of(result?.data as T);
        };
    }

    logger(status: string) {
        console.log(status);
    }

    public getSellerService<T>(url: string) {
        return this.get<T>(this.sellerBaseUrl + url);
    }

    public getBuyerService<T>(url: string) {
        return this.get<T>(this.buyerBaseUrl + url);
    }

    public postBuyerService<T>(url: string, data: any) {
        return this.post<T>(this.buyerBaseUrl + url, data);
    }

    public postSellerService<T>(url: string, data: any) {
        return this.post<T>(this.sellerBaseUrl + url, data);
    }

    public deleteSellerService<T>(url: string) {
        return this.delete<T>(this.sellerBaseUrl + url);
    }

    //#region Private

    private get<T>(url: string) {
        return this.httpclient.get<ApiResponse<T>>(url)
            .pipe(
                retry(2),
                map((resp: ApiResponse<T>) => {
                    return resp.data;
                }),
                tap(_ => this.logger('fetching compeleted')),
                catchError(this.handleError<T>("Get Operation"))
            );
    }

    private post<T>(url: string, data: any) {
        return this.httpclient.post<ApiResponse<T>>(url, JSON.stringify(data), httpOptions)
            .pipe(
                retry(2),
                map((resp: ApiResponse<T>) => {
                    return resp.data;
                }),
                tap(_ => this.logger('fetching compeleted')),
                catchError(this.handleError<T>("Post Operation"))
            );
    }

    private delete<T>(url: string) {
        return this.httpclient.delete<ApiResponse<T>>(url)
            .pipe(
                retry(2),
                map((resp: ApiResponse<T>) => {
                    return resp.data;
                }),
                tap(_ => this.logger('fetching compeleted')),
                catchError(this.handleError<T>("Delete Operation"))
            );
    }

    //#endregion Private
}