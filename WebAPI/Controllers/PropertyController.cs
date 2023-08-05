using System;
using System.Collections.ObjectModel;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.Data.Services.Structures;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IPhoto iphoto;

        public PropertyController(IUnitOfWork uow, IMapper mapper, IPhoto iphoto)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.iphoto = iphoto;
        }

        // property/list/1
        [HttpGet("list/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            var properties = await uow.IPropertyRepo.GetPropertiesAsync(sellRent);
            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties);
            return Ok(propertyListDto);
        }

        // property/detail/1
        [HttpGet("detail/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var property = await uow.IPropertyRepo.GetPropertyDetailsAsync(id);
            var propertyDto = mapper.Map<PropertyDetailDto>(property);
            return Ok(propertyDto);
        }

        // property/add
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            var property = mapper.Map<Property>(propertyDto);
            var userId = GetUserId();
            Console.WriteLine(userId);
            property.PostedBy =  userId;
            property.UpdatedBy = userId;

            uow.IPropertyRepo.AddProperty(property);
            await uow.SaveAsync();
            return StatusCode(201);
        }


        // property/add/photo/1
        [HttpPost("add/photo/{propId}")]
        [Authorize]
        public async Task<ActionResult<PhotoDto>> AddPropertyPhoto(IFormFile file, int propId)
        {
            var result = await iphoto.UploadPhotoAsync(file, propId);

            if(result.StatusCode != 201)
                return BadRequest(result.Message);

            var property = await uow.IPropertyRepo.GetPropertyPhotosByIdAsync(propId);

            var photo = new Photo {
                ImageUrl = propId + file.FileName,
                PropertyId = propId   
            };

            if(property!.Photos == null || property.Photos.Count == 0){
                photo.IsPrimary = true;                
                property.Photos = new Collection<Photo>();
            }

            var userId = GetUserId();
            photo.UpdatedBy =  userId;
            photo.UpdatedOn = DateTime.Now;

            property.Photos.Add(photo);

            if(await uow.SaveAsync()) {
                photo.PresignedUrl =  iphoto.GetPreSignedURL(photo.ImageUrl);
                return mapper.Map<PhotoDto>(photo);
            }             

            return BadRequest("Some problem occurred in uploading the photo, please retry");
        }

        [HttpGet("gphoto")]
        [AllowAnonymous]
        public async Task<string> UploadPhotoGetAsync()
        {
           var result = await iphoto.UploadPhotoGetAsync();
            return result!;
        }

        // property/set-primary-photo/1
        [HttpPost("set-primary-photo/{propId}/{filename}")]
        [Authorize]
        public async Task<IActionResult> SetPrimaryPhoto(int propId, string filename)
        {
            var userId = GetUserId();

            var property = await uow.IPropertyRepo.GetPropertyPhotosByIdAsync(propId);
            
            if(property == null)
                return BadRequest("No such property or photo exists");

            if(property.PostedBy != userId)
                return BadRequest("You are not authorized to change the photo");
            

            var photo = property!.Photos!.FirstOrDefault(x=>x.ImageUrl == filename);

            if(photo == null)
                return BadRequest("No such property or photo exists");

            if(photo.IsPrimary)    
                return BadRequest("This is already a primary photo");

            var currentPrimary = property!.Photos!.FirstOrDefault(x=> x.IsPrimary);

            if(currentPrimary != null) 
                currentPrimary.IsPrimary = false;

            photo.IsPrimary = true;

            if(await uow.SaveAsync()) return NoContent();

            return BadRequest("Some error has occured, failed to set primary photo");

        }

        
        // property/delete-photo/1/filename
        [HttpDelete("delete-photo/{propId}/{filename}")]
        [Authorize]
        public async Task<IActionResult> DeletePhoto(int propId, string filename)
        {
            var userId = GetUserId();

            var property = await uow.IPropertyRepo.GetPropertyPhotosByIdAsync(propId);
            
            if(property == null)
                return BadRequest("No such property or photo exists");

            if(property.PostedBy != userId)
                return BadRequest("You are not authorized to delete the photo");
            

            var photo = property!.Photos!.FirstOrDefault(x=>x.ImageUrl == filename);

            if(photo == null)
                return BadRequest("No such property or photo exists");

            if(photo.IsPrimary)    
                return BadRequest("You cannot delete the primary photo");

            //remove photo from the cloud first
            var result = await iphoto.DeletePhotoAsync(propId, filename);
            if(result.StatusCode != 204)
                return BadRequest(result.Message);
            //remove photo information from the database second
            property!.Photos!.Remove(photo);            
            if(await uow.SaveAsync()) return Ok();
            
            return BadRequest("Some error has occured, failed to delete photo");

        }
    }
}