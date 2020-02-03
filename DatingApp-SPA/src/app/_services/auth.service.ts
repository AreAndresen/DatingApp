import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // lager base url for authetication i api
  baseUrl = 'http://localhost:5000/api/auth/';

  // legger inn http client i construktøren
  constructor(private http: HttpClient) { }

  // lager en login metode i auth.service som benyttes i servicen
  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
        map((response: any) => {
          const user = response; // inne i denne const user kommer token vi henter fra post http
          if (user) {
            localStorage.setItem('token', user.token); // user.token matcher det vi får fra object returneres fra API
          }
        })
      );
  }

  // ny metode for register - Her trenger vi ikke Token fordi vi ikke trenger authorization da denne er innvilget i login
  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model); // http.post er en observable, må subscribes til, gjør vi i register.component.ts
  }
}
