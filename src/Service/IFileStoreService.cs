using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace S3Bucket.Service
{
    public interface IFileStoreService
    {
        Task<FileStoreResult> PutFile(Stream file, string bucketKey);
    }
}
