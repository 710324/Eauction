import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProgressSpinnerComponent } from './component/progress-spinner.component';
import { OverlayService } from './services/overlay.service';

@NgModule({
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    OverlayModule
  ],
  declarations: [ProgressSpinnerComponent],
  exports: [ProgressSpinnerComponent],
  providers: [OverlayService]
})
export class ProgressSpinnerModule { }