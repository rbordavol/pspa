import { Component, OnInit } from '@angular/core';
import { HousingService } from '../../services/housing.service';
import { ActivatedRoute } from '@angular/router';
import { IPropertyBase } from 'src/app/models/ipropertybase';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css']
})
export class PropertyListComponent implements OnInit {
  SellRent = 1;

  properties!: IPropertyBase[];

  constructor(private route : ActivatedRoute, private housingService: HousingService) { }

  ngOnInit(): void {
    /*this.hhtp.get('data/properties.json').subscribe(
      data=>{
        this.properties=data;
      //console.log(data);
      }
    );*/

    if(this.route.snapshot.url.toString()){
      this.SellRent = 2;  //Means we are on rent-property URL else we are on base URL
    }


    this.housingService.getAllProperties(this.SellRent).subscribe(
      data => {
        this.properties = data;

        /*const newProperty = JSON.parse(localStorage.getItem('newProp') || '{}');

        if(newProperty.SellRent == this.SellRent){
          this.properties = [newProperty, ...this.properties];
        }*/

        console.log(data);
        console.log(this.route.snapshot.url.toString());
        console.log(this.SellRent);
      }  
    );
  }

}
