import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any; // må lage denne for å kunne legge daten vi får fra get values og any som kan være alt


  // må legge til private http: HttpClient for å kunne gjøre http request mot API server
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues(); // caller componenten når den intializer
  }

  // lager denne metoden for å kunne gette vårt Http endpoint med for å hente values fra serveren i API
  // for å få ut enne dataen må vi subscribe på en observable so mer en stream med data i API
  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
        this.values = response;
    }, error => {
      console.log(error);
    });
  }

}
