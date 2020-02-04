import { logging } from 'protractor';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}; // denne skal lagre username og password som vi igjen kan pass til en annen metode som sender mot server

  constructor(private authService: AuthService) { } // må legge inn private authService : AuthService i konstuktøren for å kunne benytte denne servicen fra _services

  ngOnInit() {
  }

  // lager en metode for login
  login() {
    // legger inn dette for å få tak i authServicen i login
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
      console.log(error);
    }); // authService.login returnerer en observable og man må alltid subscribe på observables
  }

  // lager en loggedIn metode for å spesifisere token og returnere true eller false, utifra om token er tom eller ikke
  // dette skal rendres for å vise velkommen msg om bruker er logget inn eller ikke
  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // !! er en kort versjon for en if metode
  }

  // lager en metode for logout
  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
