using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace S3Bucket.Service
{
    public class FileRetrievalService : IFileRetrievalService
    {        
        private readonly IFileStoreService _fileStoreService;
        private readonly IAmazonS3 _s3Client;

        public FileRetrievalService(                     
            IFileStoreService fileStoreService,            
            IAmazonS3 s3Client)
        {     
            _fileStoreService = fileStoreService;
            _s3Client = s3Client;
        }

        public FileStream GetFileAsync()
        {
            throw new NotImplementedException();
        }

        public string GetTimeSensitiveURL(FileStoreResult fileStoreResult, int timePeriodInMins)
        {
            //https://docs.aws.amazon.com/AmazonS3/latest/dev/ShareObjectPreSignedURLDotNetSDK.html           

            //int.TryParse(_settingsHelper.GetMaxURLExpireTimeInMins(), out int result);

            var result = 10;

            if (timePeriodInMins > result)
            {
                return "Time period can't exceed " + result + " mins!";
            }

            //todo alter time element as config.
            var request1 = new GetPreSignedUrlRequest
            {
                BucketName = fileStoreResult.BucketName,
                Key = fileStoreResult.BucketKey,
                Expires = DateTime.Now.AddMinutes(timePeriodInMins)
            };            

            return _s3Client.GetPreSignedURL(request1);
        }

        public ICollection<string> ListFiles()
        {
            throw new NotImplementedException();
        }
    }
}
