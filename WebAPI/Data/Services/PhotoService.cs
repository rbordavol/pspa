using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using WebAPI.Data.Services.Structures;
using AWSSDK;
using Microsoft.Extensions.Options;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;

namespace WebAPI.Data.Services.Repositories
{
    public class PhotoService : IPhoto
    {
        private readonly S3Service s3Client;
        private readonly AWSOptions options;

        public PhotoService( S3Service s3Client, IOptions<AWSOptions> options)
        {
            this.s3Client = s3Client;
            this.options = options.Value;
        }

        async Task<S3ResponseDto> IPhoto.DeletePhotoAsync(int propId, string filename)
        {
                var bucketName = this.options.BucketName; 
                
                var result = new S3ResponseDto { StatusCode = 500, Message = "Error deleting file!" };
                result = await s3Client.DeleteFileAsync(filename, bucketName);

                return result;
        }

        string IPhoto.GetPreSignedURL(string fileName)
        {
            var preSignedUrl = s3Client.GetPreSignedURL(fileName);

            return preSignedUrl??"";
        }

        async Task<S3ResponseDto> IPhoto.UploadPhotoAsync(IFormFile file, int propId)
        {
            var result = new S3ResponseDto { StatusCode = 500, Message = "No file selected!" };

            if (file.Length > 0)
            {
                var bucketName = this.options.BucketName;
                using var stream = file.OpenReadStream();                

                var fileName = propId + file.FileName;

                result = await s3Client.UploadFileAsync(fileName, stream, bucketName!);

            } else {
                result = new S3ResponseDto { StatusCode = 500, Message = "No file selected!" };
            }

            return result;
        }




        
        async Task<string> IPhoto.UploadPhotoGetAsync()
        {



            var options = this.options;
                var bucketName = this.options.BucketName;
                bucketName = bucketName + " " + this.options.AccessKey;
                bucketName = bucketName + " " + this.options.AccessSecret;

                return bucketName;

        }

    }
}