import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { UpdateLogStatusComponent } from './update-log-status/update-log-status.component';

const routes: Routes = [
  {
    path: 'update-log',
    component: UpdateLogStatusComponent,
  },
  {
    path: 'delete-temp',
    component: DeleteTempVariableNumberComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
