import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './views/login/login.component';
import { MainComponent } from './views/main/main.component';
import { EmployeesComponent } from './views/main/views/employees/employees.component';
import { SystemSettingsComponent } from './views/main/views/system-settings/system-settings.component';
import { UsersComponent } from './views/main/views/system-settings/views/users/users.component';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DialogsModule } from '@progress/kendo-angular-dialog';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';
import { IconsModule } from '@progress/kendo-angular-icons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { NotificationModule } from '@progress/kendo-angular-notification';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UserServiceInterceptor } from './http-interceptors/user.service.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { HeaderComponent } from './views/main/views/header/header.component';
import { FooterComponent } from './views/main/views/footer/footer.component';
import { ToolbarComponent } from './shared/components/toolbar/toolbar.component';
import { ResetPasswordComponent } from './views/main/views/system-settings/views/users/reset-password/reset-password.component';
import { LabelModule } from '@progress/kendo-angular-label';
import { TooltipModule } from '@progress/kendo-angular-tooltip';
import { WarningDialogComponent } from './shared/components/warning-dialog/warning-dialog.component';
import { SearchComponent } from './shared/components/search/search.component';
// Dinamic Form
import { FormActionsComponent } from './shared/components/dynamic-form/components/form-actions/form-actions.component';
import { FormBooleanComponent } from './shared/components/dynamic-form/components/form-boolean/form-boolean.component';
import { FormColorComponent } from './shared/components/dynamic-form/components/form-color/form-color.component';
import { FormDateComponent } from './shared/components/dynamic-form/components/form-date/form-date.component';
import { FormDropDownComponent } from './shared/components/dynamic-form/components/form-drop-down/form-drop-down.component';
import { FormEditorComponent } from './shared/components/dynamic-form/components/form-editor/form-editor.component';
import { FormExtrasComponent } from './shared/components/dynamic-form/components/form-extras/form-extras.component';
import { FormLabelComponent } from './shared/components/dynamic-form/components/form-label/form-label.component';
import { FormMultiSelectComponent } from './shared/components/dynamic-form/components/form-multi-select/form-multi-select.component';
import { FormMultilineTextComponent } from './shared/components/dynamic-form/components/form-multiline-text/form-multiline-text.component';
import { FormNumberComponent } from './shared/components/dynamic-form/components/form-number/form-number.component';
import { FormPasswordComponent } from './shared/components/dynamic-form/components/form-password/form-password.component';
import { FormRadioComponent } from './shared/components/dynamic-form/components/form-radio/form-radio.component';
import { FormSeparatorComponent } from './shared/components/dynamic-form/components/form-separator/form-separator.component';
import { FormSliderComponent } from './shared/components/dynamic-form/components/form-slider/form-slider.component';
import { FormTextComponent } from './shared/components/dynamic-form/components/form-text/form-text.component';
import { FormTimeComponent } from './shared/components/dynamic-form/components/form-time/form-time.component';
import { FormUrlComponent } from './shared/components/dynamic-form/components/form-url/form-url.component';
import { DialogConfirmComponent } from './shared/components/dialog-confirm/dialog-confirm.component';
import { NotificationBodyComponent } from './shared/components/notification-body/notification-body.component';
import { MenuButtonComponent } from './shared/components/menu-button/menu-button.component';
import { DialogFormComponent } from './shared/components/dialog-form/dialog-form.component';
import { PanelComponent } from './shared/components/panel/panel.component';
import { DynamicFormComponent } from './shared/components/dynamic-form/dynamic-form.component';
import { PlaceholderComponent } from './shared/components/placeholder/placeholder.component';
import { DndModule } from 'ngx-drag-drop';
import { EditorModule } from '@progress/kendo-angular-editor';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { AutoFocusDirective } from './shared/directives/auto-focus.directive';
import { SortPipe } from './shared/pipes/sort.pipe';
import { FilterPipe } from './shared/pipes/filter.pipe';
import { BooleanFilterComponent } from './shared/components/boolean-filter/boolean-filter.component';
import { MulticheckFilterComponent } from './shared/components/multicheck-filter/multicheck-filter.component';
import { NewEmployeesDialogComponent } from './views/main/views/employees/dialogs/new-employee-dialog/new-employees-dialog.component';
import { NewTestComponent } from './views/main/views/employees/dialogs/new-test/new-test.component';

@NgModule({
  declarations: [
    // Directives
    AutoFocusDirective,
    // Pipes
    SortPipe,
    FilterPipe,
    AppComponent,
    LoginComponent,
    MainComponent,
    EmployeesComponent,
    SystemSettingsComponent,
    UsersComponent,
    SpinnerComponent,
    FooterComponent,
    HeaderComponent,
    ToolbarComponent,
    ResetPasswordComponent,
    WarningDialogComponent,
    SearchComponent,
    // Dynamic Form
    DynamicFormComponent,
    FormActionsComponent,
    FormBooleanComponent,
    FormColorComponent,
    FormDateComponent,
    FormDropDownComponent,
    FormEditorComponent,
    FormExtrasComponent,
    FormLabelComponent,
    FormMultiSelectComponent,
    FormMultilineTextComponent,
    FormNumberComponent,
    FormPasswordComponent,
    FormRadioComponent,
    FormSeparatorComponent,
    FormSliderComponent,
    FormTextComponent,
    FormTimeComponent,
    FormUrlComponent,
    // Dialog confirm
    DialogConfirmComponent,
    // Dialog Form
    DialogFormComponent,
    // Notification
    NotificationBodyComponent,
    MenuButtonComponent,
    PanelComponent,
    PlaceholderComponent,
    BooleanFilterComponent,
    MulticheckFilterComponent,
    NewEmployeesDialogComponent,
    NewTestComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    ButtonsModule,
    DialogsModule,
    DropDownsModule,
    GridModule,
    IconsModule,
    InputsModule,
    LayoutModule,
    LabelModule,
    NotificationModule,
    TooltipModule,
    EditorModule,
    DateInputsModule,
    // Ngx
    DndModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: UserServiceInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
