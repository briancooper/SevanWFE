using Workflow.Storage.Models;

namespace Workflow.Storage.Configuration
{
    public class StorageConfiguration : IStorageConfiguration
    {
        public AmazonSettings Amazon { get; set; }

        public AzureSettings Azure { get; set; }
    }
}
