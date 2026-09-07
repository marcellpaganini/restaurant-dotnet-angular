import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MealsComponent } from './pages/meals/meals.component';
import { ClientsComponent } from './pages/clients/clients.component';
import { OrdersComponent } from './pages/orders/orders.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'meals' },
  { path: 'meals', component: MealsComponent },
  { path: 'clients', component: ClientsComponent },
  { path: 'orders', component: OrdersComponent },
  { path: '**', redirectTo: 'meals' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
