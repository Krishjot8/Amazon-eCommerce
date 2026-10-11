import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerRegister } from 'src/app/models/accounts/CustomerUserAccount/AccountRegistration/register.model';


@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = 'https://localhost:44366/api/CustomerAccount/register';

  constructor(private http: HttpClient) {}

  registerUser(userData: CustomerRegister): Observable<any> {
    return this.http.post<any>(this.apiUrl, userData);
  }
}
