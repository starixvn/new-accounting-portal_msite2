import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
  imports: [MatButtonModule, MatDialogModule, MatSnackBarModule],
  exports: [MatButtonModule, MatDialogModule, MatSnackBarModule],
})
export class AngularMaterialModule {}
