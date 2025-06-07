// src/app/views/pages/bookings/bookings.component.ts
import { Component, OnInit }     from '@angular/core';
import { CommonModule }           from '@angular/common';
import { HttpClientModule }       from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { FormsModule }            from '@angular/forms';

import { BookingService, Booking } from '../../../services/booking.service';
import { UserService, User }       from '../../../services/user.service';
import { FlightService, Flight }   from '../../../services/flight.service';
import { RoleService, Role }       from '../../../services/role.service';
import { AirportService, Airport } from '../../../services/airport.service';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.scss']
})
export class BookingsComponent implements OnInit {
  bookings: Booking[] = [];
  users:    User[]    = [];
  flights:  Flight[]  = [];
  roles:    Role[]    = [];
  airports: Airport[] = []; 

  loading  = false;
  showForm = false;
  isEdit   = false;
  success  = '';
  error    = '';
  currentId: number|null = null;
  customerRoleId!: number;
  bookingForm!: FormGroup;

  serviceClasses = ['Economy','Business','First'] as const;

  selectedUserText   = '';
  userOpen           = false;
  filteredUsers:  User[] = [];

  selectedFlightText = '';
  flightOpen         = false;
  filteredFlights: Flight[] = [];

  constructor(
    private fb:   FormBuilder,
    private svc:  BookingService,
    private usvc: UserService,
    private fsvc: FlightService,
    private airportSvc: AirportService,
    private rsvc: RoleService
  ){}

  ngOnInit(): void {

    this.bookingForm = this.fb.group({
      userId:       [null, [Validators.required]],
      flightId:     [null, [Validators.required]],
      seatNumber:   ['',   [Validators.required]],
      serviceClass: ['Economy', [Validators.required]]
    });

    this.rsvc.getAll().subscribe({
      next: roles => {
        this.roles = roles;
        const custRole = this.roles.find(r => r.roleName === 'Customer');
        this.customerRoleId = custRole?.roleId ?? -1;

        this.usvc.getAll().subscribe({
          next: u => {
            this.users = u.filter(user => user.roleId === this.customerRoleId);
            this.filteredUsers = [...this.users];
          },
          error: e => console.error('Failed to load users', e)
        });
      },
      error: e => console.error('Failed to load roles', e)
    });

    this.fsvc.getAll().subscribe({
      next: f => {
        this.flights = f;
        this.filteredFlights = [...this.flights];
        this.airportSvc.getAll().subscribe(a => this.airports = a);
      },
      error: e => console.error('Failed to load flights', e)
    });

    this.load();
  }

  load() {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => { this.bookings = data; this.loading = false; },
      error: err => {
        this.error = 'Could not load bookings.'; this.loading = false;
      }
    });
  }

  onUserChange(term: string) {
    this.selectedUserText = term;
    this.userOpen = true;
    const t = term.toLowerCase();
    this.filteredUsers = this.users.filter(u =>
      u.username.toLowerCase().includes(t) ||
      `${u.firstName} ${u.lastName}`.toLowerCase().includes(t)
    );
  }
  selectUser(u: User, ev: MouseEvent) {
    ev.preventDefault();
    this.selectedUserText = `${u.firstName} ${u.lastName}`;
    this.bookingForm.get('userId')!.setValue(u.userId);
    this.userOpen = false;
  }
  onUserBlur() { setTimeout(()=> this.userOpen=false, 200); }

  onFlightChange(term: string) {
    this.selectedFlightText = term;
    this.flightOpen = true;
    const t = term.toLowerCase();
    this.filteredFlights = this.flights.filter(f =>
      f.flightNumber.toLowerCase().includes(t)
    );
  }

  selectFlight(f: Flight, ev: MouseEvent) {
    ev.preventDefault();
    this.selectedFlightText = f.flightNumber;
    this.bookingForm.get('flightId')!.setValue(f.flightId);
    this.flightOpen = false;
  }

  onFlightBlur() { setTimeout(()=> this.flightOpen=false, 200); }

  openCreate() {
    this.isEdit = false; this.currentId = null;
    this.bookingForm.reset({ userId:null, flightId:null, seatNumber:'', serviceClass:'Economy' });
    this.selectedUserText = '';
    this.selectedFlightText = '';
    this.filteredUsers   = [...this.users];
    this.filteredFlights = [...this.flights];
    this.showForm = true;
  }

  openEdit(b: Booking) {
    this.isEdit = true; this.currentId = b.bookingId||null;

    const u = this.users.find(x=>x.userId===b.userId)||null;
    const f = this.flights.find(x=>x.flightId===b.flightId)||null;

    this.bookingForm.setValue({
      userId:       b.userId,
      flightId:     b.flightId,
      seatNumber:   b.seatNumber,
      serviceClass: b.serviceClass
    });
    this.selectedUserText   = u?`${u.firstName} ${u.lastName}`:'';
    this.selectedFlightText = f?f.flightNumber:'';
    this.filteredUsers   = [...this.users];
    this.filteredFlights = [...this.flights];
    this.showForm = true;
  }

  save() {
    this.error = this.success = '';
    if (this.bookingForm.invalid) {
      this.error = 'Please correct form errors.'; return;
    }

    const fv = this.bookingForm.value as {
      userId: number;
      flightId: number;
      seatNumber: string;
      serviceClass: 'Economy'|'Business'|'First';
    };

    const flight = this.flights.find(f => f.flightId === fv.flightId);
    if (!flight) {
      this.error = 'Invalid flight selected.';
      return;
    }
    const bookingDateIso = new Date(flight.departureTime).toISOString();

    const payload: any = {
      userId: fv.userId,
      flightId: fv.flightId,
      seatNumber: fv.seatNumber,
      serviceClass: fv.serviceClass,
      bookingDate: bookingDateIso,
      createdAt: new Date().toISOString()
    };

    if (this.isEdit && this.currentId!=null) {
      payload.bookingId = this.currentId;
      this.svc.update(this.currentId, payload).subscribe({
        next:()=>{ this.success='Booking updated!'; this.load(); this.showForm=false; },
        error:()=>this.error='Update failed.'
      });
    } else {
      this.svc.create(payload).subscribe({
        next:()=>{ this.success='Booking created!'; this.load(); this.showForm=false; },
        error:()=>this.error='Create failed.'
      });
    }
  }

  remove(id:number) {
    if(!confirm('Delete this booking?')) return;
    this.svc.delete(id).subscribe({
      next:()=>{ this.success='Booking deleted'; this.load(); },
      error:()=>this.error='Delete failed.'
    });
  }

  cancel(){ this.showForm=false; }

  getUserName(id: number): string {
    const u = this.users.find(x => x.userId === id);
    return u ? `${u.firstName} ${u.lastName}` : '—';
  }

  getFlightNumber(id: number): string {
    const f = this.flights.find(x => x.flightId === id);
    return f ? f.flightNumber : '—';
  }

  getAirportCode(id: number): string {
  const a = this.airports.find(x => x.airportId === id);
  return a ? a.code : '—';
}
}
