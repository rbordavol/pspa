import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/models/iphoto';
import { Property } from 'src/app/models/properties';
import { HousingService } from 'src/app/services/housing.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() property!: Property;
  //@Output() mainPhotoChangeEvent = new EventEmitter<string>();
  uploader!: FileUploader;
  baseUrl = environment.baseUrl;
  maxAllowedFileSize = 10*1024*1024;

  constructor(private housingService: HousingService) { }

  ngOnInit(): void {
    this.initializeFileUploader();
  }

  // mainPhotoChanged(url: string){
  //   this.mainPhotoChangeEvent.emit(url);
  // }

  initializeFileUploader(){
    this.uploader = new FileUploader({
      url: this.baseUrl + '/property/add/photo/'+ String(this.property.id),
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: true,
      maxFileSize: this.maxAllowedFileSize
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response){
        const photo = JSON.parse(response);
        this.property.photos?.push(photo);
      }
    };
  }

  setPrimaryPhoto(propertyId: number, photo: Photo){
    this.housingService.setPrimaryPhoto(propertyId, photo.imageUrl).subscribe(()=>{
      //this.mainPhotoChanged(photo.presignedUrl);
      this.property.photos?.forEach(p=>{
        if(p.isPrimary)
        {
          p.isPrimary = false;
        }
        if(p.imageUrl === photo.imageUrl)
        {
          p.isPrimary = true;
        }
      })
    });
  }

  deletePhoto(propertyId: number, photo: Photo){
    this.housingService.deletePhoto(propertyId, photo.imageUrl).subscribe(()=>{

      this.property.photos = this.property.photos?.filter(p=>
        p.imageUrl !== photo.imageUrl );

    });
  }
}
