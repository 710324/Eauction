import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { IProductService } from 'src/app/interface';
import { ProductModel } from 'src/app/model';
import { ProductDeleteDialogComponent } from '../product-delete-dialog/product-delete-dialog.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {

  constructor(private productService: IProductService, public dialog: MatDialog) { }

  productList: ProductModel[];
  displayedColumns: string[] = ['productName', 'shortDeceription', 'detailedDeceription', 'category', 'bidEndDate', 'startingPrice', 'view', 'delete'];

  ngOnInit(): void {
    this.getAllProduct();
  }

  getAllProduct() {
    this.productService.viewAllProducts().subscribe(response => {
      this.productList = response;
    });
  }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, productId: string): void {
    this.dialog.open(ProductDeleteDialogComponent, {
      width: '50%',
      enterAnimationDuration,
      exitAnimationDuration,
      data: productId
    }).afterClosed().subscribe((data: boolean) => {
      if (data) {
        this.productService.deleteProduct(productId).subscribe(data => {
          if (data) {
            console.log(productId + " Product deleted");
          }
          else {
            console.log(productId + " Product not deleted");
          }
          this.getAllProduct();
        });
      }
    });
  }
}
