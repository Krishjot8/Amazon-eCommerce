import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './pages/home/home.component';
import { GiftCardsComponent } from './pages/gift-cards/gift-cards.component';
import { BestSellersComponent } from './pages/best-sellers/best-sellers.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AmazonBasicsComponent } from './pages/amazon-basics/amazon-basics.component';
import { CustomerServiceComponent } from './pages/customer-service/customer-service.component';
import { PrimeComponent } from './pages/prime/prime.component';
import { NewReleasesComponent } from './pages/new-releases/new-releases.component';
import { BooksComponent } from './pages/books/books.component';
import { MusicComponent } from './pages/music/music.component';
import { RegistryComponent } from './pages/registry/registry.component';
import { AmazonHomeComponent } from './pages/amazon-home/amazon-home.component';
import { FashionComponent } from './pages/fashion/fashion.component';
import { MatIconModule } from '@angular/material/icon';
import { FooterComponent } from './components/footer/footer.component';
import { CartComponent } from './components/cart/cart.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MoversShakersComponent } from './pages/movers-shakers/movers-shakers.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { VideoGamesComponent } from './pages/video-games/video-games.component';
import { VideoGameCarouselComponent } from './components/carousels/video-game-carousel/video-game-carousel.component';
import { RegisterComponent } from './components/account/customer-account/register/register.component';
import { CustomerLoginComponent } from './components/account/customer-account/login/login.component';
import { BusinessRegisterComponent } from './components/account/business-account/business-register/business-register.component';
import { BusinessVerifyEmailComponent } from './components/account/business-account/business-verify-email/business-verify-email.component';
import { SellerLoginComponent } from './components/account/seller-account/seller-login/seller-login.component';
import { SellerRegisterComponent } from './components/account/seller-account/seller-register/seller-register.component';
import { SellerVerifyEmailComponent } from './components/account/seller-account/seller-verify-email/seller-verify-email.component';
import { BusinessLoginComponent } from './components/account/business-account/business-login/business-login.component';
import { LoginPasswordComponent } from './components/account/customer-account/login-password/login-password.component';
import { CustomerVerificationComponent } from './components/account/customer-account/customer-verification/customer-verification.component';
import { NewCustomerAccountComponent } from './components/account/customer-account/new-customer-account/new-customer-account.component';
import { AmazonHaulComponent } from './pages/amazon-haul/amazon-haul.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    GiftCardsComponent,
    BestSellersComponent,
    AmazonBasicsComponent,
    CustomerServiceComponent,
    PrimeComponent,
    NewReleasesComponent,
    BooksComponent,
    MusicComponent,
    RegistryComponent,
    AmazonHomeComponent,
    FashionComponent,
    CustomerLoginComponent,
    FooterComponent,
    RegisterComponent,
    CartComponent,
    MoversShakersComponent,
    VideoGamesComponent,
    VideoGameCarouselComponent,
    BusinessLoginComponent,
    BusinessRegisterComponent,
    BusinessVerifyEmailComponent,
    SellerLoginComponent,
    SellerRegisterComponent,
    SellerVerifyEmailComponent,
    LoginPasswordComponent,
    CustomerVerificationComponent,
    NewCustomerAccountComponent,
    AmazonHaulComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatMenuModule,
    MatListModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
