import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DexService {
  private apiUrl = `${environment.apiUrl}/vdi-dex`;
  private credentials = btoa('vendsys:NFsZGmHAGWJSZ#RuvdiV');

  constructor(private http: HttpClient) { }

  sendDexFile(machine: string, dexContent: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Basic ${this.credentials}`,
      'Content-Type': 'application/json'
    });

    return this.http.post(this.apiUrl, { machine, dexContent }, { headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';
    
    if (error.status === 0) {
      errorMessage = 'A network error occurred. Please check your connection and try again.';
    } else if (error.status === 401) {
      errorMessage = 'Unauthorized. Please check your credentials.';
    } else if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    return throwError(() => new Error(errorMessage));
  }
} 