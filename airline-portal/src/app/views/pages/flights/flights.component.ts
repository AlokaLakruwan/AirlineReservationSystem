import { Component, OnInit } from '@angular/core';
import { CommonModule }             from '@angular/common';
import { HttpClientModule }         from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { FormsModule }              from '@angular/forms';

import { FlightService, Flight }    from '../../../services/flight.service';
import { AirportService, Airport }  from '../../../services/airport.service';
import { AirplaneService, Airplane } from '../../../services/airplane.service';


@Component({
  selector: 'app-flights',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.scss']
})
export class FlightsComponent implements OnInit {
  flights: Flight[]   = [];
  airports: Airport[] = [];
  airplanes: Airplane[] = [];
  filteredAirplanes: Airplane[] = [];
  loading            = false;
  showForm           = false;
  isEdit             = false;
  success            = '';
  error              = '';
  currentId: number | null = null;
  selectedAirplaneText = '';
  airplaneOpen         = false;

  flightForm!: FormGroup;

  selectedOriginText = '';
  selectedDestText   = '';
  originOpen  = false;
  destOpen    = false;
  filteredOrigins: Airport[] = [];
  filteredDests:   Airport[] = [];

  constructor(
    private fb:         FormBuilder,
    private svc:        FlightService,
    private airportSvc: AirportService,
    private airplaneSvc:  AirplaneService 
  ) {}

  ngOnInit(): void {
    this.flightForm = this.fb.group({
      flightNumber:         ['', [Validators.required]],
      departureTime:        ['', [Validators.required]],
      arrivalTime:          ['', [Validators.required]],
      originAirportId:      [null, [Validators.required]],
      destinationAirportId: [null, [Validators.required]],
      airplaneId:           [null, [Validators.required, Validators.min(1)]]
    });

    this.loadFlights();

    this.airportSvc.getAll().subscribe(list => {
      this.airports = list;
      this.filteredOrigins = [...this.airports];
      this.filteredDests   = [...this.airports];
    });

    this.airportSvc.getAll().subscribe(list => {
      this.airports = list;
      this.filteredOrigins = [...this.airports];
      this.filteredDests   = [...this.airports];
    });

    this.airplaneSvc.getAll().subscribe(list => {
      this.airplanes = list;
      this.filteredAirplanes = [...this.airplanes];
    });
  }

  onOriginChange(term: string) {
    this.selectedOriginText = term;
    this.originOpen = true;
    const t = term.toLowerCase();
    this.filteredOrigins = this.airports.filter(a =>
      a.code.toLowerCase().includes(t) ||
      a.name.toLowerCase().includes(t)
    );
  }

  onDestChange(term: string) {
    this.selectedDestText = term;
    this.destOpen = true;
    const t = term.toLowerCase();
    this.filteredDests = this.airports.filter(a =>
      a.code.toLowerCase().includes(t) ||
      a.name.toLowerCase().includes(t)
    );
  }

  private loadFlights() {
    this.loading = true;
    this.svc.getAll().subscribe({
      next: data => { this.flights = data; this.loading = false; },
      error: err => {
        this.error = 'Could not load flights.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  openCreate(): void {
    this.isEdit = false;
    this.currentId = null;
    this.flightForm.reset({
      flightNumber:         '',
      departureTime:        '',
      arrivalTime:          '',
      originAirportId:      null,
      destinationAirportId: null,
      airplaneId:           null
    });
    this.selectedOriginText = '';
    this.selectedDestText   = '';
    this.filteredOrigins = [...this.airports];
    this.filteredDests   = [...this.airports];
    this.showForm = true;
  }

  openEdit(f: Flight): void {
    this.isEdit = true;
    this.currentId = f.flightId ?? null;
    const orig = this.airports.find(a => a.airportId === f.originAirportId) ?? null;
    const dest = this.airports.find(a => a.airportId === f.destinationAirportId) ?? null;
    const plane = this.airplanes.find(a => a.airplaneId === f.airplaneId) ?? null;

    this.selectedAirplaneText = plane
      ? `${plane.tailNumber} – ${plane.model} (${plane.capacityClass})`
      : '';

    this.filteredAirplanes = [...this.airplanes];

    this.flightForm.setValue({
      flightNumber:           f.flightNumber,
      departureTime:          f.departureTime.substring(0,16),
      arrivalTime:            f.arrivalTime.substring(0,16),
      originAirportId:        f.originAirportId,
      destinationAirportId:   f.destinationAirportId,
      airplaneId:             f.airplaneId
    });

    this.selectedOriginText = orig ? `${orig.code} – ${orig.name}` : '';
    this.selectedDestText   = dest ? `${dest.code} – ${dest.name}` : '';
    this.filteredOrigins = [...this.airports];
    this.filteredDests   = [...this.airports];
    this.showForm = true;
  }

  save(): void {
    this.error = this.success = '';
    if (this.flightForm.invalid) {
      this.error = 'Please correct the form errors.';
      return;
    }

    const fv = this.flightForm.value as {
      flightNumber:         string;
      departureTime:        string;
      arrivalTime:          string;
      originAirportId:      number;
      destinationAirportId: number;
      airplaneId:           number;
    };

    const payload: any = {
      flightNumber:         fv.flightNumber,
      departureTime:        new Date(fv.departureTime).toISOString(),
      arrivalTime:          new Date(fv.arrivalTime).toISOString(),
      originAirportId:      fv.originAirportId,
      destinationAirportId: fv.destinationAirportId,
      airplaneId:           fv.airplaneId,
      createdAt:            new Date().toISOString()
    };

    if (this.isEdit && this.currentId != null) {
      payload.flightId = this.currentId;
      this.svc.update(this.currentId, payload).subscribe({
        next: () => {
          this.success = 'Flight updated!';
          this.loadFlights();
          this.showForm = false;
        },
        error: () => this.error = 'Update failed.'
      });
    } else {
      this.svc.create(payload).subscribe({
        next: () => {
          this.success = 'Flight created!';
          this.loadFlights();
          this.showForm = false;
        },
        error: () => this.error = 'Create failed.'
      });
    }
  }

  remove(id: number): void {
    if (!confirm('Delete this flight?')) return;
    this.svc.delete(id).subscribe({
      next: () => {
        this.success = 'Flight deleted';
        this.loadFlights();
      },
      error: () => this.error = 'Delete failed.'
    });
  }

  cancel(): void {
    this.showForm = false;
  }

  onOriginBlur() {
    setTimeout(() => this.originOpen = false, 200);
  }

  onDestBlur() {
    setTimeout(() => this.destOpen = false, 200);
  }

  selectOrigin(a: Airport, ev: MouseEvent) {
    ev.preventDefault();
    this.selectedOriginText = `${a.code} – ${a.name}`;
    this.flightForm.get('originAirportId')!.setValue(a.airportId);
    this.originOpen = false;
  }


  selectDest(a: Airport, ev: MouseEvent) {
    ev.preventDefault();
    this.selectedDestText = `${a.code} – ${a.name}`;
    this.flightForm.get('destinationAirportId')!.setValue(a.airportId);
    this.destOpen = false;
  }

  onAirplaneChange(term: string) {
    this.selectedAirplaneText = term;
    this.airplaneOpen = true;
    const t = term.toLowerCase();
    this.filteredAirplanes = this.airplanes.filter(a =>
      a.tailNumber.toLowerCase().includes(t) ||
      a.model.toLowerCase().includes(t)
    );
  }

  selectAirplane(a: Airplane, ev: MouseEvent) {
    ev.preventDefault();  // let this fire before blur
    this.selectedAirplaneText = `${a.tailNumber} – ${a.model} (${a.capacityClass})`;
    this.flightForm.get('airplaneId')!.setValue(a.airplaneId);
    this.airplaneOpen = false;
  }

  onAirplaneBlur() {
    setTimeout(() => this.airplaneOpen = false, 200);
  }

  getAirportCode(id: number): string {
    const a = this.airports.find(x => x.airportId === id);
    return a ? a.code : '—';
  }

  getAirplaneTail(id: number): string {
    const a = this.airplanes.find(x => x.airplaneId === id);
    return a ? a.tailNumber : '—';
  }

}
