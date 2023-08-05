import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, catchError, concatMap, of, retry, retryWhen, throwError } from "rxjs";
import { AlertifyService } from "./alertify.service";
import { Injectable } from "@angular/core";
import { ErrorCode } from '../../enums/enums';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor{

  constructor(private alertify: AlertifyService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler){
    console.log("Http Request started");
    return next.handle(request)
        .pipe(
            retryWhen(error=> this.retryRequest(error, 3)),
            catchError((error: HttpErrorResponse) => {
              const errorMessage = this.setError(error);
              console.log(error);
              this.alertify.error(errorMessage);
              const err = new Error(errorMessage);
              return throwError(() => err);
            })
        );
  }

  // Retry the request in case of error
  retryRequest(error: Observable<any>, retryCount: number): Observable<unknown>
  {
    return error.pipe(
      concatMap((checkErr: HttpErrorResponse, count: number)=>{
        console.log("here");
        if(count <= retryCount){
          switch(checkErr.status){
            case ErrorCode.serverDown:
            case ErrorCode.unathorized:
                return of(checkErr);
          }
        }

        return throwError(() => checkErr);
      })
    );
  }

  setError(error: HttpErrorResponse): string{
    let errorMessage = "Unknown error occurred";
    console.log(errorMessage);
    if(error.error instanceof ErrorEvent){
      //Client side error
      errorMessage = error.error.message;
    }else{
      //Server side error
      if(error.status === 401){
        return error.statusText;
      }

      if(error.error.errorMessage && error.status !== 0){
        errorMessage = error.error.errorMessage;
      }

      if(!error.error.errorMessage && error.error && error.status !== 0){
        errorMessage = error.error;
      }
    }
    return errorMessage;
  }
}
