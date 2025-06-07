import { Routes } from '@angular/router';
import { DefaultLayoutComponent } from './layout';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: () => import('./views/dashboard/routes').then((m) => m.routes)
      },
      {
        path: 'airports',
        loadComponent: () => import('./views/pages/airports/airports.component').then(m => m.AirportsComponent),
        data: {
          title: 'Airports'
        }
      },
      {
        path: 'airplanes',
        loadComponent: () => import('./views/pages/airplanes/airplanes.component').then(m => m.AirplanesComponent),
        data: { title: 'Airplanes' }
      },
      {
        path: 'bookings',
        loadComponent: () => import('./views/pages/bookings/bookings.component').then(m => m.BookingsComponent),
        data: {
          title: 'Bookings'
        }
      },
      {
        path: 'flights',
        loadComponent: () => import('./views/pages/flights/flights.component').then(m => m.FlightsComponent),
        data: {
          title: 'Flights'
        }
      },
      {
        path: 'users',
        loadComponent: () => import('./views/pages/users/users.component').then(m => m.UsersComponent),
        data: {
          title: 'Users'
        }
      },
      {
        path: 'reports',
        loadComponent: () => import('./views/pages/reports/reports.component').then(m => m.ReportsComponent),
        data: {
          title: 'Reports'
        }
      },
      {
        path: 'theme',
        loadChildren: () => import('./views/theme/routes').then((m) => m.routes)
      },
      {
        path: 'base',
        loadChildren: () => import('./views/base/routes').then((m) => m.routes)
      },
      {
        path: 'buttons',
        loadChildren: () => import('./views/buttons/routes').then((m) => m.routes)
      },
      {
        path: 'forms',
        loadChildren: () => import('./views/forms/routes').then((m) => m.routes)
      },
      {
        path: 'icons',
        loadChildren: () => import('./views/icons/routes').then((m) => m.routes)
      },
      {
        path: 'notifications',
        loadChildren: () => import('./views/notifications/routes').then((m) => m.routes)
      },
      {
        path: 'widgets',
        loadChildren: () => import('./views/widgets/routes').then((m) => m.routes)
      },
      {
        path: 'charts',
        loadChildren: () => import('./views/charts/routes').then((m) => m.routes)
      },
      {
        path: 'pages',
        loadChildren: () => import('./views/pages/routes').then((m) => m.routes)
      }
    ]
  },
  {
    path: '404',
    loadComponent: () => import('./views/pages/page404/page404.component').then(m => m.Page404Component),
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    loadComponent: () => import('./views/pages/page500/page500.component').then(m => m.Page500Component),
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    loadComponent: () => import('./views/pages/login/login.component').then(m => m.LoginComponent),
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    loadComponent: () => import('./views/pages/register/register.component').then(m => m.RegisterComponent),
    data: {
      title: 'Register Page'
    }
  },
  { path: '**', redirectTo: 'dashboard' }
];
