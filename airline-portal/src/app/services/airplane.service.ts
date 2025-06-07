import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Airplane {
  airplaneId?:    number;
  tailNumber:     string;
  model:          string;
  capacityClass:  'Small' | 'Medium' | 'Large';
  createdAt?:     string;
  updatedAt?:     string;
}

@Injectable({ providedIn: 'root' })
export class AirplaneService {
  private url = `${environment.apiBaseUrl}/Airplanes`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Airplane[]> {
    return this.http.get<Airplane[]>(this.url);
  }

  create(data: Partial<Airplane>): Observable<Airplane> {
    return this.http.post<Airplane>(this.url, data);
  }
  update(id: number, data: Partial<Airplane>): Observable<Airplane> {
    return this.http.put<Airplane>(`${this.url}/${id}`, data);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
