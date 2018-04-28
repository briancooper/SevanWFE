using System.Threading.Tasks;

namespace Workflow.Abstractions.Services
{
    public interface IStorageService
    {
        Task<string> AmazonS3UploadFileAsync(string filePath);
    }
}
