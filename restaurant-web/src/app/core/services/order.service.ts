import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order } from '../../models/restaurant.models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly url = `${environment.apiBaseUrl}/orders`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.url);
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.url}/${id}`);
  }
}
