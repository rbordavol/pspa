import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property } from '../models/properties';
import { environment } from '../../environments/environment'
import { ipropertytype } from '../models/ipropertytype';
import { Ifurnishingtype } from '../models/ifurnishingtype';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  baseUrl = environment.baseUrl;
  property !: Property;

  constructor(private http: HttpClient) {

    interface ApiResponse {
      [key: string] : any
    }
   }

   getAllCities() : Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + '/city/cities');
   }

   getProperty(id: number) : Observable<Property> {
    return this.http.get<Property>(this.baseUrl + '/property/detail/' + id.toString());
  }

  getAllProperties(SellRent?: number) : Observable<Property[]> {
        console.log(localStorage.getItem('token'));
  const httpOptions = {
    headers: new HttpHeaders({
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })
  };
    return this.http.get<Property[]>(this.baseUrl + '/property/list/' + SellRent?.toString(),httpOptions);
  }

  getAllPropertyTypes() : Observable<ipropertytype[]>{
    return this.http.get<ipropertytype[]>(this.baseUrl + '/propertytype/list');
  }

  getAllFurnishingTypes() : Observable<Ifurnishingtype[]>{
    return this.http.get<Ifurnishingtype[]>(this.baseUrl + '/furnishingtype/list');
  }

  addProperty(property: Property) {
    console.log(localStorage.getItem('token'));
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      })
    };

    return this.http.post(this.baseUrl + '/property/add', property, httpOptions);
  }

  addPropertyX(property: Property) {
    //let newProp = [property];

    //Add new property in array if newProp already exists in local storage
    // if(localStorage.getItem('newProp')){
    //   newProp = [property, ...JSON.parse((localStorage.getItem('newProp') || '{}'))];
    // }
    // localStorage.setItem('newProp', JSON.stringify(newProp));
  }

  newPropID(){
    if(localStorage.getItem('PID')){
      localStorage.setItem('PID', String(+(localStorage.getItem('PID') || '{}') + 1));
      return +(localStorage.getItem('PID') || '{}');
    } else {
      localStorage.setItem('PID', '101');
      return 101;
    }
  }

  getPropertyAge(dateofEstablishment: Date){
    const today = new Date();
    const estDate = new Date(dateofEstablishment);
    let age = today.getFullYear() - estDate.getFullYear();
    const mos = today.getMonth() - estDate.getMonth();

    // Current month smaller than establishment month or
    // Same month buth current date smaller than establishment date
    if(mos < 0 || (mos === 0 && today.getDate() < estDate.getDate())){
      age --;
    }

    //Establishment date is future date
    if(today < estDate){
      return '0';
    }

    //Age is less than a year
    if(age === 0){
      return "Less than a year";
    }

    return age.toString();
  }

  setPrimaryPhoto(propertyId: number, photoName: string){
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      })
    };
    return this.http.post(this.baseUrl + '/property/set-primary-photo/'+String(propertyId)
        +'/'+photoName,{},httpOptions);
  }

  deletePhoto(propertyId: number, photoName: string){
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + localStorage.getItem('token')
      })
    };
    return this.http.delete(this.baseUrl + '/property/delete-photo/'+String(propertyId)
        +'/'+photoName,httpOptions);
  }

}
