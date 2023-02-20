import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private tostr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err:HttpErrorResponse)=>{
        if(err)
        {
          switch(err.status){
            case 400:
             if(err.error.errors)
             {
                 const modelStateError=[];
                 for(const key in err.error.errors)
                 {
                   if(err.error.errors[key])
                   {
                    modelStateError.push(err.error.errors[key])
                   }
                 }
                 throw modelStateError.flat();
             }
             else{
              this.tostr.error(err.error,err.status.toString());
             }

             break;

             case 401:
              this.tostr.error("unauthorized",err.status.toString());
              break;

              case 404:
              this.router.navigateByUrl('/not-found');
              break;

              case 500:
                const navigateExtras: NavigationExtras={state:{error:err.error}}
                this.router.navigateByUrl('/server-error',navigateExtras);
                break;

                default:
                  this.tostr.error('something went wrong');
                  console.log(err);
                  break;
          }
        }
        throw err;
      })
    )
  }
}
