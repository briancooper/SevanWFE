namespace Workflow.Storage.Models
{
    public class AmazonSettings
    {
        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public Amazon.RegionEndpoint Region { get; set; }

        public string Directory { get; set; }

        public string BucketName { get; set; }
    }
}
