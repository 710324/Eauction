import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IProductService, IUserService } from 'src/app/interface';
import { BuyerBidModel, UserModel } from 'src/app/model';

@Component({
  selector: 'app-place-bids',
  templateUrl: './place-bids.component.html',
  styleUrls: ['./place-bids.component.scss']
})
export class PlaceBidsComponent implements OnInit {

  productId: string | null;

  form = new FormGroup({
    firstName: new FormControl({ value: '', disabled: true }),
    lastName: new FormControl({ value: '', disabled: true }),
    email: new FormControl('', [Validators.required, Validators.email]),
    phNumber: new FormControl({ value: '', disabled: true }),
    user: new FormControl({ value: '', disabled: true }),
    bidAmount: new FormControl('', [Validators.required]),
    address: new FormControl({ value: '', disabled: true }),
    city: new FormControl({ value: '', disabled: true }),
    state: new FormControl({ value: '', disabled: true }),
    pin: new FormControl({ value: '', disabled: true }),
    userType: new FormControl({ value: '', disabled: true })
  });

  constructor(private activeRouter: ActivatedRoute,
    private router: Router,
    private productService: IProductService,
    private userService: IUserService) { }

  ngOnInit(): void {
    this.productId = this.activeRouter.snapshot.paramMap.get('id');
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    var request = this.form.value as BuyerBidModel;
    request.productId = this.productId;
    this.productService.placeBid(request).subscribe(data => {
      this.router.navigate(['/view-product-bids']);
    });
  }

  onBlurEmail(event: any) {
    this.userService.getUserByEmail(event.target.value).subscribe((data: UserModel) => {
      if (data) {
        this.form.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          phNumber: data.phone,
          user: data.userName,
          address: data.address,
          city: data.city,
          state: data.state,
          pin: data.pin,
          userType: data.userType
        });
      }
    });
  }

}
