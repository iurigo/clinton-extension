// Kendo UI
import { NotificationSettings } from "@progress/kendo-angular-notification";

export interface ExtendedNotificationSettings extends NotificationSettings {
  hideOnClick?: boolean;
}

export interface NotificationOptions {
  hideAfter?: number;
  hideOnClick?: boolean;
}