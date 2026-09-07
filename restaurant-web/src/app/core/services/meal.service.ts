import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Meal } from '../../models/restaurant.models';

@Injectable({
  providedIn: 'root'
})
export class MealService {
  private readonly url = `${environment.apiBaseUrl}/meals`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<Meal[]> {
    return this.http.get<Meal[]>(this.url);
  }

  getById(id: number): Observable<Meal> {
    return this.http.get<Meal>(`${this.url}/${id}`);
  }
}
