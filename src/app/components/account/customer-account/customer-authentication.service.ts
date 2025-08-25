import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CustomerLogin } from 'src/app/models/accounts/customer/customer-login.model';
import { PasswordChallengeResponse } from 'src/app/models/accounts/customer/customer-password-challenge-response';

@Injectable({
  providedIn: 'root',
})
export class CustomerAuthenticationService {
  private apiUrl = `https://localhost:44366`;

  constructor(private http: HttpClient) {}

  private identifierKey = 'loginIdentifier'; //name of storagekey

  storeIdentifier(data: CustomerLogin) {
    //saves the email/phone to local storage
    localStorage.setItem(this.identifierKey, data.emailOrPhone);
  }

  getIdentifier(): string {
    //retrieves the email or phone from local storage when needed

    return localStorage.getItem(this.identifierKey) ?? ''; //?? means if null return blank '';
  }


startPasswordChallenge(payload: CustomerLogin): Observable<PasswordChallengeResponse>{

  return this.http.post<PasswordChallengeResponse>(`${this.apiUrl}/password-challenge`,payload);
}

  login(loginData: CustomerLogin): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginData).pipe(
      map((response) => {
        if (response && response.token) {
          //if success response and JWT Return, login as user.
          localStorage.setItem('currentUser', JSON.stringify(response));
        }
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('currentUser');
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('currentUser') != null;
  }
}
