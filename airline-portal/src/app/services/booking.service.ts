// src/app/services/booking.service.ts

import { Injectable }           from '@angular/core';
import { HttpClient }           from '@angular/common/http';
import { Observable }           from 'rxjs';
import { environment }          from '../../environments/environment';

export type ServiceClass = 'Economy' | 'Business' | 'First';

export interface Booking {
  bookingId?:      number;
  bookingDate:     string;          
  userId:          number;
  flightId:        number;
  seatNumber:      string;
  serviceClass:    ServiceClass;
  createdAt?:      string;           
  updatedAt?:      string;          
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private readonly url = `${environment.apiBaseUrl}/Bookings`;
  
  constructor(private http: HttpClient) {}

  getAll(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.url);
  }

  getById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.url}/${id}`);
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
