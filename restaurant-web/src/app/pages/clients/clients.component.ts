import { Component, OnInit } from '@angular/core';
import { Client } from '../../models/restaurant.models';
import { ClientService } from '../../core/services/client.service';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  loading = false;
  error = '';

  constructor(private readonly clientService: ClientService) {}

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.loading = true;
    this.error = '';
    this.clientService.getAll().subscribe({
      next: (clients) => {
        this.clients = clients;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message || 'Failed to load clients from the API.';
        this.loading = false;
      }
    });
  }
}
