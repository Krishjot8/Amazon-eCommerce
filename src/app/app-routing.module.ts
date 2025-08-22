import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmazonBasicsComponent } from './pages/amazon-basics/amazon-basics.component';
import { AmazonHomeComponent } from './pages/amazon-home/amazon-home.component';
import { BestSellersComponent } from './pages/best-sellers/best-sellers.component';
import { BooksComponent } from './pages/books/books.component';
import { CustomerServiceComponent } from './pages/customer-service/customer-service.component';
import { FashionComponent } from './pages/fashion/fashion.component';
import { GiftCardsComponent } from './pages/gift-cards/gift-cards.component';
import { HomeComponent } from './pages/home/home.component';
import { MusicComponent } from './pages/music/music.component';
import { NewReleasesComponent } from './pages/new-releases/new-releases.component';
import { PrimeComponent } from './pages/prime/prime.component';
import { RegistryComponent } from './pages/registry/registry.component';
import { MoversShakersComponent } from './pages/movers-shakers/movers-shakers.component';
import { VideoGamesComponent } from './pages/video-games/video-games.component';
import { RegisterComponent } from './components/account/customer-account/register/register.component';
import { CustomerLoginComponent } from './components/account/customer-account/login/login.component';
import { VerifyEmailComponent } from './components/account/customer-account/verify-email/verify-email.component';
import { LoginPasswordComponent } from './components/account/customer-account/login-password/login-password.component';

const routes: Routes = [
  {
    path: '',

    component: HomeComponent,
  },

  {
    path: 'gift-cards',

    component: GiftCardsComponent,
  },

  {
    path: 'bestsellers',

    component: BestSellersComponent,
  },
  {
    path: 'stores',

    component: AmazonBasicsComponent,
  },
  {
    path: 'help',

    component: CustomerServiceComponent,
  },
  {
    path: 'amazonprime',

    component: PrimeComponent,
  },
  {
    path: 'new-releases',

    component: NewReleasesComponent,
  },
  {
    path: 'books',

    component: BooksComponent,
  },
  {
    path: 'music',

    component: MusicComponent,
  },
  {
    path: 'registries',

    component: RegistryComponent,
  },
  {
    path: 'home-garden-kitchen-furniture-bedding',

    component: AmazonHomeComponent,
  },
  {
    path: 'amazon-fashion',

    component: FashionComponent,
  },
  {
    path: 'signin',

    component: CustomerLoginComponent,
  },
    {
    path: 'login-password',

    component: LoginPasswordComponent,
  },
  {
    path: 'register',

    component: RegisterComponent,
  },
  {
    path: 'movers&shakers',

    component: MoversShakersComponent,
  },
  {
    path: 'video-games',

    component: VideoGamesComponent,
  },
  {
    path: 'verify-email',
    component: VerifyEmailComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
