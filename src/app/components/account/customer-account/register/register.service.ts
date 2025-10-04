import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerRegistration } from 'src/app/models/accounts/customer/customer-register.model';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = 'https://localhost:44366/api/CustomerAccount/register';

  constructor(private http: HttpClient) {}

  registerUser(userData: CustomerRegistration): Observable<any> {
    return this.http.post<any>(this.apiUrl, userData);
  }
}
