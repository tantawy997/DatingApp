import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs';
import { PaginationResult } from '../Models/pagination';

export function getPaginationResults<T>(
  url: string,
  params: HttpParams,
  http: HttpClient
) {
  const PaginationResults: PaginationResult<T> = new PaginationResult<T>();

  return http
    .get<T>(url, {
      observe: 'response',
      params: params,
    })
    .pipe(
      map((response) => {
        //console.log(response);

        if (response.body) {
          PaginationResults.result = response.body;
        }
        const pagination = response.headers.get('pagination');
        if (pagination !== null) {
          PaginationResults.Pagination = JSON.parse(pagination);
        }
        return PaginationResults;
      })
    );
}

export function GetPaginationHeader(page: number, pagesize: number) {
  let params = new HttpParams();
  params = params.append('pageNumber', page);
  params = params.append('pageSize', pagesize);
  return params;
}
