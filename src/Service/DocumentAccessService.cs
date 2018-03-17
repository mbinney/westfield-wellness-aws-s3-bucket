using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace S3Bucket.Service
{
    public class DocumentAccessService : IDocumentAccessService
    {
        public bool CheckAccessRight()
        {
            throw new NotImplementedException();
        }

        public ICollection<string> ListFiles()
        {
            throw new NotImplementedException();
        }

        public string GetTimeSensitiveURL()
        {
             throw new NotImplementedException();
        }
    }
}
