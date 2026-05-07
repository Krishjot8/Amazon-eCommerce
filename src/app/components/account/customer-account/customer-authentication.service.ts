import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { CustomerLogin } from 'src/app/models/accounts/CustomerUserAccount/Authentication/login.model';
import { PasswordChallengeResponse } from 'src/app/models/user-authentication/password-challenge/password-challenge-response.model';
import { PasswordChallengeVerify } from 'src/app/models/user-authentication/password-challenge/password-challenge-verify.model';
import { ResendOtpRequest } from 'src/app/models/user-authentication/password-challenge/resend-otp-request.model';
import { VerifySms } from 'src/app/models/user-authentication/verification/verify-sms.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})

export class CustomerAuthenticationService {
  private apiUrl = environment.apiUrl;

private currentUserSubject = new BehaviorSubject<string | null>(localStorage.getItem('firstName')); // Initialize with firstName from localStorage if available
currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  private identifierKey = 'loginIdentifier'; //name of storagekey


setCurrentUser(firstName: string | null) {

  this.currentUserSubject.next(firstName);
}




checkIdentifier(identifier: string) {

return this.http.post<{exists: boolean}>
(`${this.apiUrl}/passwordchallenge/check-identifier`, 
  { identifier });

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
         this.handleAuthSuccess(response);

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
    return localStorage.getItem('authToken') != null;
  }

  //Email OTP

  verifyOtp(payload: PasswordChallengeVerify){
    return this.http.post<any>(
      `${this.apiUrl}/passwordchallenge/verify`,
     payload
    ).pipe(
      tap((response) => this.handleAuthSuccess(response)
    ));

  }


  resendEmailOtp(payload:ResendOtpRequest){

return this.http.post<any>(`${this.apiUrl}/passwordchallenge/resend`,payload);
  }



  verifySms(payload: VerifySms){
    return this.http.post<any>(
      `${this.apiUrl}/verification/verify-sms`,
     payload
    ).pipe(
      tap((response) => this.handleAuthSuccess(response)
    ));

  }




  resendSmsOtp(payload: VerifySms){

    return this.http.post<any>(`${this.apiUrl}/verification/resend-sms-otp`,payload);
      }
    
    
      private handleAuthSuccess(response: any) {

        const fullName = `${response.firstName ?? ''} ${response.lastName ?? ''}`.trim();

        localStorage.setItem('authToken', response.token);
        localStorage.setItem('firstName', response.firstName);
        localStorage.setItem('fullName', fullName);

        this.setCurrentUser(response.firstName);

      }

}
