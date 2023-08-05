using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using WebAPI.Dtos;

namespace WebAPI.Data.Services.Repositories
{
    public class S3Service
    {
        private readonly AWSOptions options;

        public S3Service(IOptions<AWSOptions> options)
        {
            this.options = options.Value;
        }

        
        public async Task<S3ResponseDto> DeleteFileAsync(string fileName, string bucketName)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(this.options.AccessKey, this.options.AccessSecret);
            var awsConfig = new AmazonS3Config { RegionEndpoint = Amazon.RegionEndpoint.USEast2 };

            var response = new S3ResponseDto();

            try
            {                

                var key = GetPreSignedURL(fileName);

    
                //initialize client
                using var client = new AmazonS3Client(credentials, awsConfig);

                var transferUtility = new TransferUtility(client);

                await transferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest(){
                    BucketName = bucketName,
                    Key = fileName
                });

                response.StatusCode = 204;
                response.Message = "No Content";

            }
            catch(AmazonS3Exception ex) 
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<S3ResponseDto> UploadFileAsync(string fileName, Stream stream, string bucketName)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(this.options.AccessKey, this.options.AccessSecret);
            var awsConfig = new AmazonS3Config { RegionEndpoint = Amazon.RegionEndpoint.USEast2 };

            var response = new S3ResponseDto();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = stream,
                    Key = fileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                //initialize client
                using var client = new AmazonS3Client(credentials, awsConfig);
                //initialize the transfer/upload tools
                var transferUtility = new TransferUtility(client);
                //initiate the file upload
                await transferUtility.UploadAsync(uploadRequest);
                


                response.StatusCode = 201;
                response.Message = "Photo upload successfull";

            }
            catch(AmazonS3Exception ex) 
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        public string GetPreSignedURL(string fileName){
            
            var credentials = new Amazon.Runtime.BasicAWSCredentials(this.options.AccessKey, this.options.AccessSecret);
            var awsConfig = new AmazonS3Config { RegionEndpoint = Amazon.RegionEndpoint.USEast2 };//initialize client
            
            using var client = new AmazonS3Client(credentials, awsConfig);

            var preSignedRequest = new GetPreSignedUrlRequest()
            {
                Key = fileName,
                BucketName = this.options.BucketName,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };
            var preSignedUrl = client.GetPreSignedURL(preSignedRequest);

            return preSignedUrl??"";
        }
    }
}