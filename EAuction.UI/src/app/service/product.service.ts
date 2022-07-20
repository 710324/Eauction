import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { IProductService } from '../interface';
import { ProductModel, ProductBidsModel, ProductToBuyer, BuyerBidModel, BidDetailModel } from '../model';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService implements IProductService {

  productList$: Observable<ProductModel[]>;

  product$: Observable<ProductModel | undefined>;

  productBids$: Observable<ProductBidsModel>;

  productToBuyer$: Observable<ProductToBuyer>;

  constructor(private httpService: HttpService) { }

  viewAllProducts(): Observable<ProductModel[]> {
    this.productList$ = this.httpService.getSellerService<ProductModel[]>('products');
    return this.productList$
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  viewAllProduct(productId: string | null): Observable<ProductModel | undefined> {
    return this.httpService.getSellerService<ProductModel>('productbyid/' + productId)
      .pipe(
        map(resp => {
          return resp;
        })
      );
  }

  viewAllBidForProduct(productId: string): Observable<ProductBidsModel> {
    this.productBids$ = this.httpService.getSellerService<ProductBidsModel>('show-bids/' + productId);
    return this.productBids$
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  placeBid(data: BuyerBidModel): Observable<ProductToBuyer> {
    this.productToBuyer$ = this.httpService.postBuyerService<ProductToBuyer>('place-bid', data);
    return this.productToBuyer$
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  addProduct(data: ProductModel): Observable<ProductModel> {
    return this.httpService.postSellerService<ProductModel>('addproduct', data)
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  deleteProduct(productId: string): Observable<boolean> {
    return this.httpService.deleteSellerService<boolean>('delete/' + productId);
  }

  getBidForProduct(productBidId: string | null): Observable<BuyerBidModel> {
    return this.httpService.getBuyerService<BuyerBidModel>('getproductbidbyid/' + productBidId)
      .pipe(
        map(list => {
          return list;
        })
      );
  }

  updateBid(data: BuyerBidModel): Observable<ProductToBuyer> {
    this.productToBuyer$ = this.httpService.postBuyerService<ProductToBuyer>('update-bid', data);
    return this.productToBuyer$
      .pipe(
        map(list => {
          return list;
        })
      );
  }

}
