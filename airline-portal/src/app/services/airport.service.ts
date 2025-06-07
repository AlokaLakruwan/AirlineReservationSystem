import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Airport {
  airportId?:   number;
  code:         string;
  name:         string;
  city:         string;
  country:      string;
  createdAt?:   string;
  updatedAt?:   string;
}

@Injectable({ providedIn: 'root' })
export class AirportService {
  private baseUrl  = environment.apiBaseUrl;
  private url      = `${this.baseUrl}/Airports`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.url);
  }

  create(data: Partial<Airport>): Observable<Airport> {
    return this.http.post<Airport>(this.url, data);
  }

  update(id: number, data: Partial<Airport>): Observable<Airport> {
    return this.http.put<Airport>(`${this.url}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
