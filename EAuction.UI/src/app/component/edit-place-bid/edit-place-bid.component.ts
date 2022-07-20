import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IProductService, IUserService } from 'src/app/interface';
import { BuyerBidModel, UserModel } from 'src/app/model';

@Component({
  selector: 'app-edit-place-bid.component',
  templateUrl: './edit-place-bid.component.html',
  styleUrls: ['./edit-place-bid.component.scss']
})
export class EditPlaceBidComponent implements OnInit {

  productBidId: string | null;
  productBidInfo: BuyerBidModel;


  form = new FormGroup({
    email: new FormControl({ value: '', disabled: true }),
    bidAmount: new FormControl('', [Validators.required])
  });

  constructor(private activeRouter: ActivatedRoute,
    private router: Router,
    private productService: IProductService,
    private userService: IUserService) { }

  ngOnInit(): void {
    this.productBidId = this.activeRouter.snapshot.paramMap.get('id');
    this.getProductBidId();
  }

  getProductBidId() {
    this.productService.getBidForProduct(this.productBidId).subscribe(data => {
      if (data) {
        this.form.patchValue({
          email: data.email,
          bidAmount: data.bidAmount?.toString()
        });
      }
      this.productBidInfo = data;
    });
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    this.productBidInfo.bidAmount = parseFloat(((this.form.value.bidAmount) ? this.form.value.bidAmount : "0.00"));
    this.productService.updateBid(this.productBidInfo).subscribe(data => {
      this.router.navigate(['/view-product-bids']);
    });
  }
}
