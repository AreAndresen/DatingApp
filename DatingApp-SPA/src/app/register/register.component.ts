import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // denne gir oss tilgang til alle verdiene i values til home - BENYTTES IKKE LENGER
  // @Input() valuesFromHome: any; // dette vi må bruke for å hente input verdien fra moder - component home.component

  // ALTERNATIV DATADELING FRA INPUT OVER - DENNE SENDES FRA REGISTER COMPONENT TIL HOME
  @Output() cancelRegister = new EventEmitter(); // må benytte EvenEmitter() for output

  model: any = {}; // spesifiserer ett tomt object av typen any

  // må legge inn authService for å benytte metoden vi har lagd i auth.service.ts for registrering
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  // legger ved en register metode - som nevnt må register metoden subsribes på pga at http er observable
  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('registration successful');
    }, error => {
      console.log(error); // dette blir da http responsen vi får tilbake fra serveren
    });
  }

  // legger ved en cancel metode
  cancel() {
    this.cancelRegister.emit(false); // emit (sender) og vi må spesifisere hva vi vil emitte/sende: false kan være et object en boolean e.l, en form for data
    console.log('cancelled');
  }

}
