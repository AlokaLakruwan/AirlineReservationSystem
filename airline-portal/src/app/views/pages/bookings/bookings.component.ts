import { Component, OnInit } from '@angular/core';
import { CommonModule }             from '@angular/common';
import { HttpClientModule }         from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { BookingService, Booking } from '../../../services/booking.service';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.scss']
})
export class BookingsComponent implements OnInit {
  bookings: Booking[] = [];
  loading   = false;
  success   = '';
  error     = '';
  showForm  = false;
  isEdit    = false;
  currentId: number | null = null;
  bookingForm!: FormGroup;

  // You could fetch real users/flights; for now we'll use simple numeric inputs
  serviceClasses = ['Economy', 'Business', 'First'] as const;

  constructor(
    private svc: BookingService,
    private fb:  FormBuilder
  ) {}

  ngOnInit(): void {
    this.load();
    this.bookingForm = this.fb.group({
      userId:       [null, [Validators.required, Validators.min(1)]],
      flightId:     [null, [Validators.required, Validators.min(1)]],
      seatNumber:   ['', [Validators.required]],
      serviceClass: ['Economy', [Validators.required]]
    });
  }

  load(): void {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => { this.bookings = data; this.loading = false; },
      error: err => {
        this.error = 'Could not load bookings.';
        console.error(err);
        this.loading = false;
        this.clearMsg();
      }
    });
  }

  openCreate(): void {
    this.isEdit = false;
    this.currentId = null;
    this.bookingForm.reset({
      userId:       null,
      flightId:     null,
      seatNumber:   '',
      serviceClass: 'Economy'
    });
    this.showForm = true;
  }

  openEdit(b: Booking): void {
    this.isEdit = true;
    this.currentId = b.bookingId ?? null;
    this.bookingForm.setValue({
      userId:       b.userId,
      flightId:     b.flightId,
      seatNumber:   b.seatNumber,
      serviceClass: b.serviceClass
    });
    this.showForm = true;
  }

  save(): void {
    this.error = this.success = '';
    if (this.bookingForm.invalid) {
      this.error = 'Please correct the form errors.';
      this.clearMsg();
      return;
    }
    const fv = this.bookingForm.value as Booking;
    const payload: any = {
      userId:       fv.userId,
      flightId:     fv.flightId,
      seatNumber:   fv.seatNumber,
      serviceClass: fv.serviceClass,
      bookingDate:  new Date().toISOString(),  // server will default if you omit
      createdAt:    new Date().toISOString()
    };

    if (this.isEdit && this.currentId != null) {
      payload.bookingId = this.currentId;
      this.svc.update(this.currentId, payload).subscribe({
        next: () => {
          this.success = 'Booking updated!';
          this.load();
          this.showForm = false;
          this.clearMsg();
        },
        error: e => {
          this.error = 'Update failed.';
          console.error(e);
          this.clearMsg();
        }
      });
    } else {
      this.svc.create(payload).subscribe({
        next: () => {
          this.success = 'Booking created!';
          this.load();
          this.showForm = false;
          this.clearMsg();
        },
        error: e => {
          this.error = 'Create failed.';
          console.error(e);
          this.clearMsg();
        }
      });
    }
  }

  remove(id: number): void {
    if (!confirm('Delete this booking?')) return;
    this.error = this.success = '';
    this.svc.delete(id).subscribe({
      next: () => {
        this.success = 'Booking deleted';
        this.load();
        this.clearMsg();
      },
      error: e => {
        this.error = 'Delete failed.';
        console.error(e);
        this.clearMsg();
      }
    });
  }

  cancel(): void {
    this.showForm = false;
  }

  private clearMsg(): void {
    setTimeout(() => { this.error = ''; this.success = ''; }, 3000);
  }
}
