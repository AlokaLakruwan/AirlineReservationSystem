import { Component, OnInit }       from '@angular/core';
import { CommonModule }             from '@angular/common';
import { HttpClientModule }         from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { AirplaneService, Airplane } from '../../../services/airplane.service';

@Component({
  selector: 'app-airplanes',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  templateUrl: './airplanes.component.html',
  styleUrls: ['./airplanes.component.scss']
})
export class AirplanesComponent implements OnInit {
  airplanes: Airplane[] = [];
  loading   = false;
  success   = '';
  error     = '';
  showForm  = false;
  isEdit    = false;
  currentId: number | null = null;
  airplaneForm!: FormGroup;

  capacityClasses = ['Small','Medium','Large'] as const;

  constructor(
    private svc: AirplaneService,
    private fb:  FormBuilder
  ) {}

  ngOnInit(): void {
    this.load();
    this.airplaneForm = this.fb.group({
      tailNumber:    ['', [Validators.required]],
      model:         ['', [Validators.required]],
      capacityClass: ['Medium', [Validators.required]]
    });
  }

  load(): void {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => { this.airplanes = data; this.loading = false; },
      error: err => {
        this.error = 'Could not load airplanes.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  openCreate(): void {
    this.isEdit = false;
    this.currentId = null;
    this.airplaneForm.reset({
      tailNumber:    '',
      model:         '',
      capacityClass: 'Medium'
    });
    this.showForm = true;
  }

  openEdit(a: Airplane): void {
    this.isEdit = true;
    this.currentId = a.airplaneId ?? null;
    this.airplaneForm.setValue({
      tailNumber:    a.tailNumber,
      model:         a.model,
      capacityClass: a.capacityClass
    });
    this.showForm = true;
  }

  save(): void {
    this.error = this.success = '';
    if (this.airplaneForm.invalid) {
      this.error = 'Please fix form errors.';
      return;
    }
    const fv = this.airplaneForm.value as Partial<Airplane>;
    const payload: any = {
      tailNumber:    fv.tailNumber,
      model:         fv.model,
      capacityClass: fv.capacityClass,
      createdAt:     new Date().toISOString()
    };

    if (this.isEdit && this.currentId != null) {
      payload.airplaneId = this.currentId;
      this.svc.update(this.currentId, payload).subscribe({
        next: () => {
          this.success = 'Airplane updated!';
          this.load();
          this.showForm = false;
        },
        error: e => { this.error = 'Update failed.'; console.error(e); }
      });
    } else {
      this.svc.create(payload).subscribe({
        next: () => {
          this.success = 'Airplane created!';
          this.load();
          this.showForm = false;
        },
        error: e => { this.error = 'Create failed.'; console.error(e); }
      });
    }
  }

  remove(id: number): void {
    if (!confirm('Delete this airplane?')) return;
    this.svc.delete(id).subscribe({
      next: () => {
        this.success = 'Airplane deleted';
        this.load();
      },
      error: () => this.error = 'Delete failed.'
    });
  }

  cancel(): void { this.showForm = false; }
}
