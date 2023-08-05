import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { AlertifyService } from "../services/alertify.service";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private jwtHelper: JwtHelperService,
    private alertify: AlertifyService,){

  }

  canActivate(): boolean{
    const token = localStorage.getItem('token');

    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }

    this.alertify.error('You must be first logged in to access this area');
    this.router.navigate(["/user/login"]);
    return false;
  }
}
