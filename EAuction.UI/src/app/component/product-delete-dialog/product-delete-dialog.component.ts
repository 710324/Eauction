import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-product-delete-dialog',
  templateUrl: './product-delete-dialog.component.html',
  styleUrls: ['./product-delete-dialog.component.scss']
})
export class ProductDeleteDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ProductDeleteDialogComponent>, @Inject(MAT_DIALOG_DATA) public productId: string) { }

  ngOnInit(): void {
    console.log(this.productId);
  }

  closeDialog(option: boolean) {
    this.dialogRef.close(option);
  }

}
