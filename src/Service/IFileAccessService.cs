using System;
using System.Collections.Generic;
using System.Text;

namespace S3Bucket.Service
{
    interface IFileAccessService 
    {
        bool CheckAccessRight();     
    }
}
