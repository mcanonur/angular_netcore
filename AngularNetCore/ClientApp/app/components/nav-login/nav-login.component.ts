import { Component, Inject, Input } from '@angular/core';
import { Http, Headers } from '@angular/http';

@Component({
    selector: 'nav-login',
    templateUrl: './nav-login.component.html'
})
export class NavLoginComponent {

    @Input() isLoggedIn: boolean = false;
    @Input() email: string;
    
}

