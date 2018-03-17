using System;
using System.Collections.Generic;
using System.Text;

namespace S3Bucket.Service
{
    interface IDocumentAccessService 
    {
        bool CheckAccessRight();
        ICollection<string> ListFiles();
        string GetTimeSensitiveURL();      
    }
}
