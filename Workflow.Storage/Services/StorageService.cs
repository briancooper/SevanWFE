using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using System.Threading.Tasks;
using Workflow.Abstractions.Services;
using Workflow.Storage.Configuration;

namespace Workflow.Storage.Services
{
    public class StorageService : IStorageService
    {
        private readonly IStorageConfiguration _storageConfiguration;


        public StorageService(IStorageConfiguration storageConfiguration)
        {
            _storageConfiguration = storageConfiguration;
        }

        public async Task<string> AmazonS3UploadFileAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var amazonSettings = _storageConfiguration.Amazon;

                var fileKey = $"{amazonSettings.Directory}/{fileName}";

                var fileTransferUtility = new TransferUtility(new AmazonS3Client(amazonSettings.AccessKey, amazonSettings.SecretKey, amazonSettings.Region));

                var request = new TransferUtilityUploadRequest
                {
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = amazonSettings.BucketName,
                    Key = fileKey,
                    InputStream = stream
                };

                await fileTransferUtility.UploadAsync(request);

                return $"http://{request.BucketName}.s3.amazonaws.com/{fileKey}";
            }
        }
    }
}
