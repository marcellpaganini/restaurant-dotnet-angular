import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Client } from '../../models/restaurant.models';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private readonly url = `${environment.apiBaseUrl}/clients`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<Client[]> {
    return this.http.get<Client[]>(this.url);
  }

  getById(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.url}/${id}`);
  }
}
