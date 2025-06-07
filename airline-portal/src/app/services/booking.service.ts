import { Injectable } from '@angular/core';
import { HttpClient }    from '@angular/common/http';
import { Observable }    from 'rxjs';
import { environment }   from '../../environments/environment';

export interface Booking {
  bookingId?:    number;
  bookingDate?:  string; // ISO string, server will default if omitted
  userId:        number;
  flightId:      number;
  seatNumber:    string;
  serviceClass:  'Economy' | 'Business' | 'First';
  createdAt?:    string;
  updatedAt?:    string;
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private url = `${environment.apiBaseUrl}/Bookings`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.url);
  }

  create(data: Partial<Booking>): Observable<Booking> {
    return this.http.post<Booking>(this.url, data);
  }

  update(id: number, data: Partial<Booking>): Observable<Booking> {
    return this.http.put<Booking>(`${this.url}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
