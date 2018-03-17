using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace S3Bucket.Service
{
    interface IDocumentRetrievalService
    {
        FileStream GetFileAsync();
    }
}
