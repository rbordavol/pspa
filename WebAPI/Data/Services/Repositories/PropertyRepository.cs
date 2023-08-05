using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebAPI.Data.Services.Structures;
using WebAPI.Models;

namespace WebAPI.Data.Services.Repositories
{
    public class PropertyRepository : IProperty
    {
        private readonly DataContext daContext;
        private readonly IPhoto iphoto;
        private readonly AWSOptions options;
        public PropertyRepository(DataContext daContext, IOptions<AWSOptions> options, IPhoto iphoto)
        {
            this.daContext = daContext;
            this.iphoto = iphoto;
            this.options = options.Value;
        }
        void IProperty.AddProperty(Property property)
        {
            daContext.Properties.Add(property);
        }


        void IProperty.DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<Property>> IProperty.GetPropertiesAsync(int sellRent)
        {
            var properties = await daContext.Properties
                                    .Include(p => p.PropertyType)
                                    .Include(p => p.City)
                                    .Include(p => p.FurnishingType)
                                    .Include(p => p.Photos)
                                    .Where(x=>x.SellRent == sellRent)
                                    .ToListAsync();

            if(properties != null){
                foreach(var property in properties){
                    if(property.Photos!.Count !=0){
                        foreach(var photo in property.Photos){
                            if(photo.ImageUrl != null && photo.IsPrimary == true){
                                photo.PresignedUrl = iphoto.GetPreSignedURL(photo.ImageUrl);
                            }
                        }
                    } else {
                        property.Photos = new Collection<Photo>();
                        property.Photos.Add(new Photo
                        {
                            ImageUrl = "default",
                            IsPrimary = true,
                            PresignedUrl = iphoto.GetPreSignedURL("house1.jpg")
                        });
                    }
                }
            }
            return properties;
        }

        async Task<Property> IProperty.GetPropertyDetailsAsync(int id)
        {
            
            var property = await daContext.Properties
                                    .Include(p => p.PropertyType)
                                    .Include(p => p.City)
                                    .Include(p => p.FurnishingType)
                                    .Include(p => p.Photos)
                                    .Where(x => x.Id == id)
                                    .FirstAsync();

            if(property != null){
                if(property.Photos != null){
                    foreach(var photo in property.Photos){
                        if(photo.ImageUrl != null){
                         photo.PresignedUrl = iphoto.GetPreSignedURL(photo.ImageUrl);
                        }
                    }
                } else {
                    property.Photos = new Collection<Photo>();
                    property.Photos.Add(new Photo
                     {
                        ImageUrl = "default",
                        IsPrimary = true,
                        PresignedUrl = iphoto.GetPreSignedURL("house1.jpg")
                    });
                }
            }

            return property;
            
        }
        async Task<Property?> IProperty.GetPropertyPhotosByIdAsync(int id)
        {
            
            var property = await daContext.Properties
                                    .Include(p => p.Photos)
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();

            return property;
            
        }
    }
}