import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlaceBidsComponent } from './component/place-bids/place-bids.component';
import { ViewProductBidsComponent } from './component/view-product-bids/view-product-bids.component';
import { ProductListComponent } from './component/product-list/product-list.component';
import { ViewProductComponent } from './component/view-product/view-product.component';
import { NewProductComponent } from './component/new-product/new-product.component';
import { NewSellerComponent } from './component/new-seller/new-seller.component';
import { NewBuyerComponent } from './component/new-buyer/new-buyer.component';
import { UserListComponent } from './component/view-user/user-list.component';
import { EditPlaceBidComponent } from './component';

const routes: Routes = [
  { path: 'product-list', component: ProductListComponent },
  { path: 'add-product', component: NewProductComponent },
  { path: 'add-seller', component: NewSellerComponent },
  { path: 'user-list', component: UserListComponent },
  { path: 'add-buyer', component: NewBuyerComponent },
  { path: 'view-product/:id', component: ViewProductComponent },
  { path: 'view-product-bids', component: ViewProductBidsComponent },
  { path: 'place-bid/:id', component: PlaceBidsComponent },
  { path: 'edit-place-bid/:id', component: EditPlaceBidComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
