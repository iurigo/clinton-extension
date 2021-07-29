// Angular
import { Component, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.scss']
})
export class PanelComponent {
  @Input() public title: string = null;
  @Input() public iconClass: string = null;
  @Input() public isBusy: boolean = false;
  @Input() public isMicroBusy: boolean = false;
  @Input() public isDraggable: boolean = false;
  @Input() public draggableData: any = null;

  @Output() public onDragStart = new EventEmitter<void>();
  @Output() public onDragEnd = new EventEmitter<void>();
}
