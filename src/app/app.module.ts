import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { GiftCardsComponent } from './gift-cards/gift-cards.component';
import { BestSellersComponent } from './best-sellers/best-sellers.component';

import {MatToolbarModule} from '@angular/material/toolbar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AmazonBasicsComponent } from './amazon-basics/amazon-basics.component';
import { CustomerServiceComponent } from './customer-service/customer-service.component';
import { PrimeComponent } from './prime/prime.component';
import { NewReleasesComponent } from './new-releases/new-releases.component';
import { BooksComponent } from './books/books.component';
import { MusicComponent } from './music/music.component';
import { RegistryComponent } from './registry/registry.component';
import { AmazonHomeComponent } from './amazon-home/amazon-home.component';
import { FashionComponent } from './fashion/fashion.component';
import { LoginComponent } from './login/login.component';
import {MatIconModule} from '@angular/material/icon';
import { FooterComponent } from './footer/footer.component';
import { RegisterComponent } from './register/register.component';

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
    LoginComponent,
    FooterComponent,
    RegisterComponent,


  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
   MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
