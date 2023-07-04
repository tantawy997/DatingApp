export interface User {
  // UserId: string;
  userName: string;
  password?: string;
  token: string;
  photoUrl: string;
  knownAs: string;
  gender: string;
  roles: string[];
}
