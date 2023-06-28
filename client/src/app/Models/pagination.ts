export interface Pagination {
  currentPage: number;
  totalItems: number;
  totalPages: number;
  pageSize: number;
}

export class PaginationResult<T> {
  result?: T;
  Pagination?: Pagination;
}
