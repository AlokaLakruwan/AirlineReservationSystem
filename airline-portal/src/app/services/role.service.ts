import { Injectable } from '@angular/core';
import { HttpClient }    from '@angular/common/http';
import { Observable }    from 'rxjs';
import { environment }   from '../../environments/environment';

export interface Role {
  roleId:   number;
  roleName: string;
}

@Injectable({ providedIn: 'root' })
export class RoleService {
  private url = `${environment.apiBaseUrl}/Roles`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Role[]> {
    return this.http.get<Role[]>(this.url);
  }
}
