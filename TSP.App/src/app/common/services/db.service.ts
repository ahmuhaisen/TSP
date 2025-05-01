import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../types/api.types';


@Injectable({
  providedIn: 'root',
})
export class DbService {

  private http = inject(HttpClient);

  private baseUrl = environment.apiURL;

  getRequest<Response>(relativeRoute: string): Observable<Response> {
    return this.http
      .get<ApiResponse>(`${this.baseUrl}${relativeRoute}`)
      .pipe(
        map((response) => response.responseData)
      );
  }

  postRequest<Response, Payload>(
    relativeRoute: string,
    body: Payload
  ): Observable<Response> {
    console.log(`Making POST request to: ${this.baseUrl}${relativeRoute}`);
    console.log('Request payload:', body);
    
    return this.http
      .post<ApiResponse>(`${this.baseUrl}${relativeRoute}`, body)
      .pipe(
        map((response) => {
          console.log('POST response:', response);
          return response.responseData;
        })
      );
  }

  patchRequest<Response, Payload>(
    relativeRoute: string,
    body: Payload
  ): Observable<Response> {
    return this.http
      .patch<ApiResponse>(this.getUrl(relativeRoute), body)
      .pipe(
        map((response) => response.responseData)
      );
  }

  putRequest<Response, Payload>(
    relativeRoute: string,
    body: Payload
  ): Observable<Response> {
    return this.http
      .put<ApiResponse>(this.getUrl(relativeRoute), body)
      .pipe(
        map((response) => response.responseData)
      );
  }

  deleteRequest<Response>(relativeRoute: string): Observable<Response> {
    return this.http
      .delete<ApiResponse>(this.getUrl(relativeRoute))
      .pipe(
        map((response) => response.responseData)
      );
  }


  private getUrl(relativeRoute: string = '') {
    return `${this.baseUrl}${relativeRoute}`;
  }
}
