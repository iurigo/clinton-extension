// Angular
import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
// Interfaces
import { ButtonAction } from '../../interfaces/actions.interface';
// RxJS
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnChanges {

  @Input() public actions: ButtonAction[];
  @Input() public title: string;
  public availableActions: ButtonAction[];

  @Output() public actionClick = new EventEmitter<number>(null);

  private subscription$ = new Subscription();

  constructor(private _user: UserService) { }

  public ngOnChanges(): void {

    this.subscription$.add(this._user.isAdmin$.subscribe(isAdmin => {
      if (isAdmin != null) {
        if (!isAdmin) {
          this.availableActions = this.actions.filter(action => !action.adminOnly);
        } else {
            this.availableActions = this.actions.map(a => a);
          };
        };
    }));
  }

}
