using System;
using System.Collections.Generic;
using System.Text;

namespace S3Bucket.Service
{
    interface IDocumentStoreService
    {
        Boolean UploadFileAsync();
    }
}
