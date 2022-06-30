using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBUpload.Model;

namespace MongoDBUpload.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<FileUploadModel> _fileCollection;

       public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _fileCollection = database.GetCollection<FileUploadModel>(mongoDBSettings.Value.CollectionName);
        }
    }
}
