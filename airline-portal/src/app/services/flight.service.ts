import { Injectable } from '@angular/core';
import { HttpClient }    from '@angular/common/http';
import { Observable }    from 'rxjs';
import { environment }   from '../../environments/environment';

export interface Flight {
  flightId?:            number;
  flightNumber:         string;
  departureTime:        string; // ISO
  arrivalTime:          string; // ISO
  originAirportId:      number;
  destinationAirportId: number;
  airplaneId:           number;
  createdAt?:           string;
  updatedAt?:           string;
}

@Injectable({ providedIn: 'root' })
export class FlightService {
  private url = `${environment.apiBaseUrl}/Flights`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Flight[]> {
    return this.http.get<Flight[]>(this.url);
  }
  create(data: Partial<Flight>): Observable<Flight> {
    return this.http.post<Flight>(this.url, data);
  }
  update(id: number, data: Partial<Flight>): Observable<Flight> {
    return this.http.put<Flight>(`${this.url}/${id}`, data);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
