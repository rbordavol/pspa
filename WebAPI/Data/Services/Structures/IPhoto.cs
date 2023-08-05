using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Dtos;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Data.Services.Structures
{
    public interface IPhoto
    {
        Task<S3ResponseDto> UploadPhotoAsync(IFormFile file, int propId);
        Task<string> UploadPhotoGetAsync();
        string GetPreSignedURL(string fileName);

        Task<S3ResponseDto> DeletePhotoAsync(int propId, string filename);

    }
}