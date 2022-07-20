import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ProgressSpinnerService } from './service';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'EAuction.UI';

  color: ThemePalette = 'primary';
  mode: ProgressSpinnerMode = 'indeterminate';
  value = 50;
  displayProgressSpinner: boolean = false;

  constructor(private progressSpinnerService: ProgressSpinnerService) {

  }

  ngOnInit(): void {
    this.listenToSpinner();

  }

  listenToSpinner() {
    this.progressSpinnerService.spinnerStatus
      .pipe(delay(0))
      .subscribe((data) => {
        this.displayProgressSpinner = data;
      });
  }
}
