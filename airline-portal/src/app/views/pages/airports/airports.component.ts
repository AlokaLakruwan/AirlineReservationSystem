import { Component, OnInit } from '@angular/core';
import { CommonModule }             from '@angular/common';
import { HttpClientModule }         from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { AirportService, Airport } from '../../../services/airport.service';

@Component({
  selector: 'app-airports',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  templateUrl: './airports.component.html',
  styleUrls: ['./airports.component.scss']
})
export class AirportsComponent implements OnInit {
  airports: Airport[] = [];
  loading  = false;
  success  = '';
  error    = '';
  showForm = false;
  isEdit   = false;
  currentId: number | null = null;
  airportForm!: FormGroup;

  constructor(
    private svc: AirportService,
    private fb:  FormBuilder
  ) {}

  ngOnInit(): void {
    this.load();
    this.airportForm = this.fb.group({
      code:    ['', [Validators.required]],
      name:    ['', [Validators.required]],
      city:    ['', [Validators.required]],
      country: ['', [Validators.required]]
    });
  }

  load(): void {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => { this.airports = data; this.loading = false; },
      error: err => {
        this.error = 'Could not load airports.';
        console.error(err);
        this.loading = false;
        this.clearMsg();
      }
    });
  }

  openCreate(): void {
    this.isEdit = false;
    this.currentId = null;
    this.airportForm.reset({
      code: '', name: '', city: '', country: ''
    });
    this.showForm = true;
  }

  openEdit(a: Airport): void {
    this.isEdit = true;
    this.currentId = a.airportId ?? null;
    this.airportForm.setValue({
      code:    a.code,
      name:    a.name,
      city:    a.city,
      country: a.country
    });
    this.showForm = true;
  }

  save(): void {
    this.error = this.success = '';
    if (this.airportForm.invalid) {
      this.error = 'Please fix validation errors.';
      this.clearMsg();
      return;
    }
    const fv = this.airportForm.value as Airport;
    const payload: any = {
      code:    fv.code,
      name:    fv.name,
      city:    fv.city,
      country: fv.country,
      createdAt: new Date().toISOString()
    };

    if (this.isEdit && this.currentId != null) {
      payload.airportId = this.currentId;
      this.svc.update(this.currentId, payload).subscribe({
        next: () => { this.success = 'Airport updated!'; this.load(); this.showForm = false; this.clearMsg(); },
        error: e => { this.error = 'Update failed.'; console.error(e); this.clearMsg(); }
      });
    } else {
      this.svc.create(payload).subscribe({
        next: () => { this.success = 'Airport created!'; this.load(); this.showForm = false; this.clearMsg(); },
        error: e => { this.error = 'Create failed.'; console.error(e); this.clearMsg(); }
      });
    }
  }

  remove(id: number): void {
    if (!confirm('Delete this airport?')) return;
    this.error = this.success = '';
    this.svc.delete(id).subscribe({
      next: () => { this.success = 'Airport deleted'; this.load(); this.clearMsg(); },
      error: e => { this.error = 'Delete failed.'; console.error(e); this.clearMsg(); }
    });
  }

  cancel(): void {
    this.showForm = false;
  }

  private clearMsg(): void {
    setTimeout(() => { this.error = ''; this.success = ''; }, 3000);
  }
}
