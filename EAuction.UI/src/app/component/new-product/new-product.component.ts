import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IProductService } from 'src/app/interface';
import { ProductModel } from 'src/app/model';

@Component({
  selector: 'app-new-product',
  templateUrl: './new-product.component.html',
  styleUrls: ['./new-product.component.scss']
})
export class NewProductComponent implements OnInit {

  form = new FormGroup({
    productName: new FormControl('', [Validators.required]),
    shortDeceription: new FormControl('', [Validators.required]),
    detailedDeceription: new FormControl('', [Validators.required]),
    category: new FormControl('', [Validators.required]),
    bidEndDate: new FormControl('', [Validators.required]),
    startingPrice: new FormControl('', [Validators.required])
  });

  constructor(private router: Router, private productService: IProductService) { }

  ngOnInit(): void {
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    var request = this.form.value as ProductModel;
    this.productService.addProduct(request).subscribe(data => {
      this.router.navigate(['/product-list']);
    });
  }


}
