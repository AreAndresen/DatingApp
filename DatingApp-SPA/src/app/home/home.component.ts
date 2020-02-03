import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false; // boolean for register mode
  // values: any; // benyttes for å samle values dataen

  constructor(private http: HttpClient) { } // legger inn private http: HttpClient for å hente values

  ngOnInit() {
    // this.getValues(); // legger til metoden som henter values på on initialization
  }

  // legger ved denne metoden som skal benyttes i en aktiveringsknapp for register mode
  registerToggle() {
    this.registerMode = true;
  }



  /*  BENYTTES IKKE LENGER - MEN TAR VARE PÅ DEN ENN SÅ LENGE
  lager denne metoden for å kunne gette vårt Http endpoint med for å hente values fra serveren i API
  // for å få ut enne dataen må vi subscribe på en observable so mer en stream med data i API
  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }*/

  // er metoden for output som sendes fra child-register til home-parent
  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }

}
