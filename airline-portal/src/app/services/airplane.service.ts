import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Airplane {
  airplaneId:    number;
  tailNumber:    string;
  model:         string;
  capacityClass: 'Small' | 'Medium' | 'Large';
}

@Injectable({ providedIn: 'root' })
export class AirplaneService {
  private url = `${environment.apiBaseUrl}/Airplanes`;
  constructor(private http: HttpClient) {}
  getAll(): Observable<Airplane[]> {
    return this.http.get<Airplane[]>(this.url);
  }
}
