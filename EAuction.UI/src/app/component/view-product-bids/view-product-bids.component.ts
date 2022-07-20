import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { IProductService } from 'src/app/interface';
import { BidDetailModel, ProductBidsModel, ProductModel } from 'src/app/model';

@Component({
  selector: 'app-view-product-bids',
  templateUrl: './view-product-bids.component.html',
  styleUrls: ['./view-product-bids.component.scss']
})
export class ViewProductBidsComponent implements OnInit {

  productList: ProductModel[];
  productBidsModel: ProductBidsModel | null;
  bidDetailModel: BidDetailModel[] | [];
  displayedColumns: string[] = ['bidAmount', 'buyerName', 'emailId', 'phone', 'view'];
  productSelected: any;

  constructor(private productService: IProductService) { }

  ngOnInit(): void {
    this.getAllProduct();
  }

  getAllProduct() {
    this.productService.viewAllProducts().subscribe(response => {
      this.productList = response;
    });
  }

  getProductBids(productId: string) {
    this.productService.viewAllBidForProduct(productId).subscribe(data => {
      if (data != null) {
        this.productBidsModel = data;
        this.bidDetailModel = this.productBidsModel.bidDetails;
      } else {
        this.productBidsModel = null;
        this.bidDetailModel = [];
      }
    });
  }

}
