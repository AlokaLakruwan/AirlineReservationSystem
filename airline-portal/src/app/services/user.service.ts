import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface User {
  userId: number;
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  roleId: number;
  isActive: boolean;
  createdAt: string;  
  updatedAt?: string; 
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = environment.apiBaseUrl;         
  private usersUrl = `${this.baseUrl}/Users`; 

  constructor(private http: HttpClient) { }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.usersUrl}/${id}`);
  }

  create(data: Partial<User>): Observable<User> {
    return this.http.post<User>(this.usersUrl, data);
  }

  update(id: number, data: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.usersUrl}/${id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.usersUrl}/${id}`);
  }
}
