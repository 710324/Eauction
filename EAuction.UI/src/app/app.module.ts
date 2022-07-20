import { NgModule, enableProdMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ToastrModule } from 'ngx-toastr';

import { ConfigModule, ConfigService } from './config/config.service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import {
  IUserService,
  IProductService
} from './interface';

import {
  ProductService,
  UserService,
  ProgressSpinnerService
} from './service';

import {
  ProductListComponent,
  UserListComponent,
  PlaceBidsComponent,
  ViewProductComponent,
  ProductDeleteDialogComponent,
  NewProductComponent,
  NewSellerComponent,
  NewBuyerComponent,
  ViewProductBidsComponent,
  EditPlaceBidComponent
} from 'src/app/component';

import { ProgressSpinnerModule } from 'src/app/progress-spinner/progress-spinner.module';
import { InterceptorService } from './service/Interceptor.service';


const MatModules = [
  MatNativeDateModule,
  MatTableModule,
  MatFormFieldModule,
  MatSelectModule,
  MatInputModule,
  MatIconModule,
  MatDialogModule,
  MatDatepickerModule,
  MatProgressSpinnerModule
]

enableProdMode();

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    UserListComponent,
    ViewProductBidsComponent,
    PlaceBidsComponent,
    ViewProductComponent,
    ProductDeleteDialogComponent,
    NewProductComponent,
    NewSellerComponent,
    NewBuyerComponent,
    EditPlaceBidComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    ...MatModules,
    ProgressSpinnerModule,
    ToastrModule.forRoot()
  ],
  providers: [
    ConfigService,
    { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true },
    ProgressSpinnerService,
    ConfigModule.init(),
    { provide: IUserService, useClass: UserService },
    { provide: IProductService, useClass: ProductService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
