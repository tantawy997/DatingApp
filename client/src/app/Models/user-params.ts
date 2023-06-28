import { User } from './user';

export class UserParams {
  gender: string;
  pageSize = 5;
  pageNumber = 1;
  maxAge = 99;
  minAge = 18;
  orderBy = 'lastActive';
  /**
   *
   */
  constructor(user: User) {
    this.gender = '';
  }
}
