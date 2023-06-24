import { CanDeactivateFn } from '@angular/router';
import { EditComponent } from '../member/edit/edit.component';

export const preventBeforeSaveGuard: CanDeactivateFn<EditComponent> = (
  component,
  currentRoute,
  currentState,
  nextState
) => {
  if (component.editForm?.dirty) {
    return confirm(
      'are you sure you want to continue any changes made will be lost'
    );
  }
  return true;
};
