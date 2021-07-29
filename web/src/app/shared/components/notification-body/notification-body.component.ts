// Angular
import { Component, EventEmitter, Input, Output } from '@angular/core';
// Enums
import { NotificationType } from './notification-body.enums';


@Component({
  selector: 'app-notification-body',
  templateUrl: './notification-body.component.html',
  styleUrls: ['./notification-body.component.scss']
})
export class NotificationBodyComponent {

  @Input() public type: NotificationType = NotificationType.INFO;
  @Input() public title: string = 'Title';
  @Input() public message: string = 'Message';

  @Output() public onClick = new EventEmitter<void>();

  public notificationType = NotificationType;
}
