import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';

@Component({
    selector: 'register',
    templateUrl: './register.component.html'
})
export class RegisterComponent {
    public email: string;
    public password: string;
    public confirmPassword: string;
    private _http: Http;
    private _baseUrl: string;
    public formErrors: string[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
    }

    register() {
        var headers = new Headers();
        //this.createAuthorizationHeader(headers);
        headers.append('Content-Type', 'application/json');

        var content = JSON.stringify({
            Email: this.email,
            Password: this.password,
            ConfirmPassword: this.confirmPassword
        });

        return this._http.post(
            this._baseUrl + 'api/Account/Register', content, {
                headers: headers
            }).map(res => res.json()).subscribe(
            data => {
                console.log(data.result);
                if (!data.isSuccess) {
                    this.formErrors = data.result;
                } else {
                    this.formErrors = [];
                } },
            err => { console.log(err); }
        );

        
    }

    
}

