//westfield-wellness-finance / src / Finance.Services / FIleStore / FileStoreService.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;

namespace S3Bucket.Service
{
    public class FileStoreService : IFileStoreService
    {
        private string _bucketName { get; set; }
        private ITransferUtility _transferUtil;
        private ILogger<FileStoreService> _logger;


        public FileStoreService(string bucketName, ITransferUtility transferUtility, ILogger<FileStoreService> logger)
        {
            _bucketName = bucketName;
            _transferUtil = transferUtility;
            _logger = logger;

        }

        public async Task<FileStoreResult> PutFile(Stream file, string bucketKey)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = file,
                Key = bucketKey,
                BucketName = _bucketName,
                CannedACL = S3CannedACL.Private,
            };

            await _transferUtil.UploadAsync(uploadRequest);

            return new FileStoreResult()
            {
                BucketKey = bucketKey,
                BucketName = _bucketName,
            };
        }
    }
}