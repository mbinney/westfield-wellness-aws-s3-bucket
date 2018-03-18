using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace S3Bucket.Service
{
    interface IFileRetrievalService
    {
        FileStream GetFileAsync();
        ICollection<string> ListFiles();
        string GetTimeSensitiveURL(FileStoreResult fileStoreResult, int timePeriodInMins);
    }
}
