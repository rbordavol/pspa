import {Component, Input } from '@angular/core';
import { IProperty } from '../IProperty.interface';
import { IPropertyBase } from 'src/app/models/ipropertybase';

@Component({
    selector: 'app-property-card',
    //template: `<h1>I am a card</h1>`
    templateUrl: 'property-card.component.html',
    //styles: ['h1 {font-weight: normal;}'],
    styleUrls: ['property-card.component.css']
})

export class PropertyCardComponent{
    @Input() property!: IPropertyBase;
    @Input() hideIcons!: boolean;

   /* Property: any = {
        "Id":1,
        "Name":"Birla House",
        "Type":"House",
        "Price":12000
    }*/

}