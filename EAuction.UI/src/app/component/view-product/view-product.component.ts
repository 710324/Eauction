import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductModel } from 'src/app/model';
import { ProductService } from 'src/app/service';

@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.component.html',
  styleUrls: ['./view-product.component.scss']
})
export class ViewProductComponent implements OnInit {

  productId: string | null;
  product: ProductModel | undefined;

  constructor(private router: ActivatedRoute, private productService: ProductService) { }

  ngOnInit(): void {
    this.productId = this.router.snapshot.paramMap.get('id');
    this.getProductById();
  }

  getProductById() {
    this.productService.viewAllProduct(this.productId).subscribe(data => {
      this.product = data;
    });
  }

}
