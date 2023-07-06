import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmazonBasicsComponent } from './amazon-basics/amazon-basics.component';
import { AmazonHomeComponent } from './amazon-home/amazon-home.component';
import { BestSellersComponent } from './best-sellers/best-sellers.component';
import { BooksComponent } from './books/books.component';
import { CustomerServiceComponent } from './customer-service/customer-service.component';
import { FashionComponent } from './fashion/fashion.component';
import { GiftCardsComponent } from './gift-cards/gift-cards.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MusicComponent } from './music/music.component';
import { NewReleasesComponent } from './new-releases/new-releases.component';
import { PrimeComponent } from './prime/prime.component';
import { RegistryComponent } from './registry/registry.component';
import { RegisterComponent } from './register/register.component';

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

    component: LoginComponent,
  },
  {
    path: 'register',

    component: RegisterComponent,
  },
];





@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
