import { Injectable, APP_INITIALIZER, isDevMode } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class ConfigService {

    private _config: any;
    private _env: string;

    constructor(private _http: HttpClient) { }
    load() {
        return new Promise((resolve, reject) => {
            this._env = (isDevMode()) ? 'development' : 'production';
            console.log(this._env);
            this._http.get('./assets/config/' + this._env + '.json')
                .pipe(
                    map((response) => {
                        return response;
                    }),
                    catchError((error) => {
                        console.error(error);
                        return of(null);
                    })
                ).subscribe((data: any) => {
                    this._config = data;
                    resolve(true);
                });
        });
    }

    // Gets API route based on the provided key
    getApi(key: string): string {
        return this._config.API_ENDPOINTS[key];
    }
    // Gets a value of specified property in the configuration file
    get(key: any) {
        return this._config[key];

    }
}

export function ConfigFactory(config: ConfigService) {
    return () => config.load();
}

export function init() {
    return {
        provide: APP_INITIALIZER,
        useFactory: ConfigFactory,
        deps: [ConfigService],
        multi: true
    }
}

const ConfigModule = {
    init: init
}

export { ConfigModule };