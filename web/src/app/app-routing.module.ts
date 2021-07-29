import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainGuard } from './guards/main.guard';
import { LoginComponent } from './views/login/login.component';
import { MainComponent } from './views/main/main.component';
import { EmployeesComponent } from './views/main/views/employees/employees.component';
import { SystemSettingsComponent } from './views/main/views/system-settings/system-settings.component';
import { UsersComponent } from './views/main/views/system-settings/views/users/users.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: MainComponent,
    canActivate: [MainGuard],
    children: [
      { path: 'employees', component: EmployeesComponent },
      {
        path: 'system-settings',
        component: SystemSettingsComponent,
        children: [
          { path: 'users', component: UsersComponent }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
