using Workflow.Storage.Models;

namespace Workflow.Storage.Configuration
{
    public interface IStorageConfiguration
    {
        AmazonSettings Amazon { get; }

        AzureSettings Azure { get; }
    }
}
