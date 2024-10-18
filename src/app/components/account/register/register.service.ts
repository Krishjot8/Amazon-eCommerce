import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserRegistration } from 'src/app/models/register.model';

@Injectable({
  providedIn: 'root',
})
export class RegisterService {
  private apiUrl = 'https://localhost:44366/api/register';

  constructor(private http: HttpClient) {}

  registerUser(userData: UserRegistration): Observable<any> {
    return this.http.post<any>(this.apiUrl, userData);
  }
}
