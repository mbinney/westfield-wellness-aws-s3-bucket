using System;
using System.Collections.Generic;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Bucket.Model
{
    public class S3
    {
        public string bucketName { get; set; }
        public string bucketKey { get; set; }        
        public string delimeter { get; set; }
        public string directory { get; set; }
        public bool accessDenied { get; set; }
    }
}
