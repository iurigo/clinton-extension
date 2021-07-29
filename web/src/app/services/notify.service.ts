// Angular
import { Injectable, EventEmitter } from '@angular/core';
import { first } from 'rxjs/operators';
// Kendo UI
import { NotificationService } from "@progress/kendo-angular-notification";
// Enums
import { NotificationType } from '../shared/components/notification-body/notification-body.enums';
// Components
import { NotificationBodyComponent } from '../shared/components/notification-body/notification-body.component';
// Interfaces
import { ExtendedNotificationSettings } from '../shared/components/notification-body/notification-body.interfaces';


@Injectable({
  providedIn: 'root'
})
export class NotifyService {

  private defaultSettings: ExtendedNotificationSettings = {
    content: NotificationBodyComponent,
    animation: { type: 'fade', duration: 800 },
    position: { horizontal: 'right', vertical: 'bottom' },
    hideAfter: 5000, // 5 seconds
    hideOnClick: true
  };

  constructor(private _notification: NotificationService) { }

  public success(message: string, title: string = null, options: NotificationOptions = {}): EventEmitter<void> {
    return this.showNotification(NotificationType.SUCCESS, 'notification-success', title || 'Success', message, options);
  }

  public error(message: string, title: string = null, options: NotificationOptions = {}): EventEmitter<void> {
    return this.showNotification(NotificationType.ERROR, 'notification-error', title || 'Error', message, options);
  }

  public warning(message: string, title: string = null, options: NotificationOptions = {}): EventEmitter<void> {
    return this.showNotification(NotificationType.WARNING, 'notification-warning', title || 'Warning', message, options);
  }

  public info(message: string, title: string = null, options: NotificationOptions = {}): EventEmitter<void> {
    return this.showNotification(NotificationType.INFO, 'notification-info', title || 'Info', message, options);
  }

  private showNotification(type: NotificationType, cssClass: string, title: string = null, message: string, options: NotificationOptions = {}): EventEmitter<void> {
    // Build configuration
    const config = {
      ...this.defaultSettings,
      ...options,
      cssClass: cssClass
    };

    // Show notification
    const popup = this._notification.show(config);

    // Configure notification
    const popupInstance: NotificationBodyComponent = popup.content.instance;
    popupInstance.type = type;
    popupInstance.title = title;
    popupInstance.message = message;

    // Close popup on click if needed
    if (config.hideOnClick) {
      popupInstance.onClick.pipe(first()).subscribe(() => popup.hide());
    }

    return popupInstance.onClick;
  }

}
