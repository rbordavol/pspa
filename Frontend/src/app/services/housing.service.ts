import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IProperty } from '../property/IProperty.interface';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { IPropertyBase } from '../models/ipropertybase';
import { Property } from '../models/properties.model';

@Injectable({
  providedIn: 'root'
})
export class HousingService {

  property !: Property;

  constructor(private http: HttpClient) {

    interface ApiResponse {
      [key: string] : any
    }
   }

   getAllCities() : Observable<string[]> {
    return this.http.get<string[]>('http://localhost:5265/api/city');
   }

   getProperty(id: number) : Observable<Property> {
    return this.getAllProperties().pipe(
      map(propertiesArray => {
        let prop = propertiesArray.find(p => p.Id === id)!;
        console.log(prop);
        return propertiesArray.find(p => p.Id === id)!;
        /*let foundProp =  (propertiesArray.find(p => p.Id === id));


        if(foundProp){
          let prop: Record<string, any> = foundProp;
          let cntx = Object.keys(prop).length;
          console.log('getProperty count ' + cntx);
          console.log(prop);*/

          //for(let ii=0; ii<cntx; ii++){
            //console.log(ii);
            //console.log(Object.keys(prop)[ii]);
            //console.log(Object.values(prop)[ii]);

           /* let key = Object.keys(prop)[ii];
            var value = prop[key];
            console.log(value);*/
/*console.log(Number(prop[Object.keys(prop)[0]]));
this.property = new Property();
            this.property.Id = Number(prop[Object.keys(prop)[0]]);
            this.property.SellRent= Number(prop[Object.keys(prop)[1]]);
            this.property.Name= String(prop[Object.keys(prop)[2]]);
            this.property.PType= String(prop[Object.keys(prop)[3]]);
            this.property.FType= String(prop[Object.keys(prop)[4]]);
            this.property.Price= (prop[Object.keys(prop)[5]]);
            this.property.BHK= Number(prop[Object.keys(prop)[6]]);
            this.property.BuiltArea= Number(prop[Object.keys(prop)[7]]);
            this.property.City= String(prop[Object.keys(prop)[8]]);
            this.property.RTM= Number(prop[Object.keys(prop)[9]]);
            this.property.Image= String(prop[Object.keys(prop)[10]]);

        }
        console.log(this.property);
        return this.property;*/
      })
    );

  }
  getAllProperties(SellRent? : number) : Observable<Property[]> {
    return this.http.get('data/properties.json').pipe(

      map(data => {
          const propertiesArray : Array<Property> = [];
          const localProperties = JSON.parse((localStorage.getItem('newProp') || '{}'));

          if(localProperties){
            let cntx = Object.keys(localProperties).length;
            console.log('newProp count ' + cntx);

            for(let ii=0; ii<cntx; ii++){
              console.log(ii);
              console.log(localProperties);
              console.log(localProperties[ii].Name);

              if(SellRent){
                if(localProperties.hasOwnProperty(ii) && Number(localProperties[ii].SellRent) === SellRent){
                  propertiesArray.push({
                    "Id": Number(localProperties[ii].Id),
                  "SellRent": Number(localProperties[ii].SellRent),
                  "BHK": Number(localProperties[ii].BHK),
                  "PType": String(localProperties[ii].PType),
                  "Name": String(localProperties[ii].Name),
                  "City": String(localProperties[ii].City),
                  "FType": String(localProperties[ii].FType),
                  "Price": Number(localProperties[ii].Price),
                  Security: Number(localProperties[ii].Security),
                  "RTM": Number(localProperties[ii].Maintenance),
                  "BuiltArea": Number(localProperties[ii].BuiltArea),
                  "CarpetArea": Number(localProperties[ii].CarpetArea),
                  "FloorNo": String(localProperties[ii].FloorNo),
                  "TotalFloor": String(localProperties[ii].TotalFloor),
                  "Address": String(localProperties[ii].Address),
                  "Address2": String(localProperties[ii].Address2),
                  "AOP": String(localProperties[ii].AOP),
                  "Gated": Number(localProperties[ii].Gated),
                  "MainEntrance": String(localProperties[ii].MainEntrance),
                  "Possession": String(localProperties[ii].Possession),
                  "Description": String(localProperties[ii].Description),
                  "PostedOn": String(localProperties[ii].PostedOn),
                  "Image": String(localProperties[ii].Image),
                  PostedBy: 0
                  });
                }
              } else {
                propertiesArray.push({
                  "Id": Number(localProperties[ii].Id),
                  "SellRent": Number(localProperties[ii].SellRent),
                  "BHK": Number(localProperties[ii].BHK),
                  "PType": String(localProperties[ii].PType),
                  "Name": String(localProperties[ii].Name),
                  "City": String(localProperties[ii].City),
                  "FType": String(localProperties[ii].FType),
                  "Price": Number(localProperties[ii].Price),
                  Security: Number(localProperties[ii].Security),
                  "RTM": Number(localProperties[ii].Maintenance),
                  "BuiltArea": Number(localProperties[ii].BuiltArea),
                  "CarpetArea": Number(localProperties[ii].CarpetArea),
                  "FloorNo": String(localProperties[ii].FloorNo),
                  "TotalFloor": String(localProperties[ii].TotalFloor),
                  "Address": String(localProperties[ii].Address),
                  "Address2": String(localProperties[ii].Address2),
                  "AOP": String(localProperties[ii].AOP),
                  "Gated": Number(localProperties[ii].Gated),
                  "MainEntrance": String(localProperties[ii].MainEntrance),
                  "Possession": String(localProperties[ii].Possession),
                  "Description": String(localProperties[ii].Description),
                  "PostedOn": String(localProperties[ii].PostedOn),
                  "Image": String(localProperties[ii].Image),
                  PostedBy: 0
                });
              }
            }
          }

          let cnt = Object.keys(data).length;

          for(let ii=0; ii<cnt; ii++){
            console.log(cnt);
            console.log(ii);
            console.log(Object.values(data)[ii]);
            console.log(Object.values(data)[ii].Name);
            if(SellRent){
              if(data.hasOwnProperty(ii) && Number(Object.values(data)[ii].SellRent) === SellRent){
                propertiesArray.push({
                  Id: Number(Object.values(data)[ii].Id),
                  SellRent: Number(Object.values(data)[ii].SellRent),
                  BHK: Number(Object.values(data)[ii].BHK),
                  PType: String(Object.values(data)[ii].PType),
                  Name: String(Object.values(data)[ii].Name),
                  City: String(Object.values(data)[ii].Name),
                  FType: String(Object.values(data)[ii].Name),
                  Price: Number(Object.values(data)[ii].Name),
                  Security: Number(Object.values(data)[ii].Name),
                  Maintenance: Number(Object.values(data)[ii].Name),
                  BuiltArea: Number(Object.values(data)[ii].Name),
                  CarpetArea: Number(Object.values(data)[ii].Name),
                  FloorNo: String(Object.values(data)[ii].Name),
                  TotalFloor: String(Object.values(data)[ii].Name),
                  Address: String(Object.values(data)[ii].Name),
                  Address2: String(Object.values(data)[ii].Name),
                  RTM: Number(Object.values(data)[ii].Name),
                  AOP: String(Object.values(data)[ii].Name),
                  Gated: Number(Object.values(data)[ii].Name),
                  MainEntrance: String(Object.values(data)[ii].Name),
                  Possession: String(Object.values(data)[ii].Name),
                  Description: String(Object.values(data)[ii].Name),
                  PostedOn: String(Object.values(data)[ii].Name),
                  Image: "",
                  PostedBy: 0
                });
              }
            } else {
              propertiesArray.push({
                Id: Number(Object.values(data)[ii].Id),
                SellRent: Number(Object.values(data)[ii].SellRent),
                BHK: Number(Object.values(data)[ii].BHK),
                PType: String(Object.values(data)[ii].PType),
                Name: String(Object.values(data)[ii].Name),
                City: String(Object.values(data)[ii].Name),
                FType: String(Object.values(data)[ii].Name),
                Price: Number(Object.values(data)[ii].Name),
                Security: Number(Object.values(data)[ii].Name),
                Maintenance: Number(Object.values(data)[ii].Name),
                BuiltArea: Number(Object.values(data)[ii].Name),
                CarpetArea: Number(Object.values(data)[ii].Name),
                FloorNo: String(Object.values(data)[ii].Name),
                TotalFloor: String(Object.values(data)[ii].Name),
                Address: String(Object.values(data)[ii].Name),
                Address2: String(Object.values(data)[ii].Name),
                RTM: Number(Object.values(data)[ii].Name),
                AOP: String(Object.values(data)[ii].Name),
                Gated: Number(Object.values(data)[ii].Name),
                MainEntrance: String(Object.values(data)[ii].Name),
                Possession: String(Object.values(data)[ii].Name),
                Description: String(Object.values(data)[ii].Name),
                PostedOn: String(Object.values(data)[ii].Name),
                Image: "",
                PostedBy: 0
              });
            }
          }

          return propertiesArray;
      })
    );
  }

  addProperty(property: Property) {
    let newProp = [property];

    //Add new property in array if newProp already exists in local storage
    if(localStorage.getItem('newProp')){
      newProp = [property, ...JSON.parse((localStorage.getItem('newProp') || '{}'))];
    }
    localStorage.setItem('newProp', JSON.stringify(newProp));
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
}
