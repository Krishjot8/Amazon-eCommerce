import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { CustomerLogin } from 'src/app/models/accounts/customer/customer-login.model';
import { CustomerOtpVerification } from 'src/app/models/accounts/customer/customer-otp-verification.model';
import { PasswordChallengeResponse } from 'src/app/models/accounts/customer/customer-password-challenge-response';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CustomerAuthenticationService {
  private apiUrl = environment.apiUrl;

private currentUserSubject = new BehaviorSubject<string | null>(localStorage.getItem('username'));
currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  private identifierKey = 'loginIdentifier'; //name of storagekey


setCurrentUser(username: string | null) {

  this.currentUserSubject.next(username);
}

  storeIdentifier(data: CustomerLogin) {
    //saves the email/phone to local storage
    localStorage.setItem(this.identifierKey, data.emailOrPhone);
  }

  getIdentifier(): string {
    //retrieves the email or phone from local storage when needed

    return localStorage.getItem(this.identifierKey) ?? ''; //?? means if null return blank '';
  }

  startPasswordChallenge(
    payload: CustomerLogin
  ): Observable<PasswordChallengeResponse> {
    return this.http.post<PasswordChallengeResponse>(
      `${this.apiUrl}/customeraccount/login`,
      payload
    );
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
   
    localStorage.clear();
    this.setCurrentUser(null);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('currentUser') != null;
  }

  resendOtp(payload: CustomerOtpVerification){

return this.http.post<any>(`${this.apiUrl}/passwordchallenge/resendotp`,payload);
  }

  verifyOtp(customerOtpVerification: CustomerOtpVerification) {
    return this.http.post<any>(`${this.apiUrl}/passwordchallenge/verify`,customerOtpVerification)
    .pipe(
      tap((response) =>{

        localStorage.setItem('authToken', response.token);
        localStorage.setItem('firstName', response.firstName);

      })
    );

  }
}
