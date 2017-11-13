import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';
import { Router } from '@angular/router';

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    public email: string;
    public password: string;
    private _http: Http;
    private _baseUrl: string;
    public formErrors: string[];
    private router: Router;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string, _router: Router) {
        this._http = http;
        this._baseUrl = baseUrl;
        this.router = _router;
    }

    login() {
        var headers = new Headers();
        //this.createAuthorizationHeader(headers);
        headers.append('Content-Type', 'application/json');

        var content = JSON.stringify({
            Email: this.email,
            Password: this.password
        });

        return this._http.post(
            this._baseUrl + 'api/Account/Login', content, {
                headers: headers
            }).map(res => res.json()).subscribe(
            data => {
                console.log(data.result);
                if (!data.isSuccess) {
                    this.formErrors = data.result;
                }
                else {
                    //TODO(can):Emit an event so nav-login component can be changed
                    this.router.navigateByUrl('home');
                }},
            err => { console.log(err); }
        );

        
    }

    
}

