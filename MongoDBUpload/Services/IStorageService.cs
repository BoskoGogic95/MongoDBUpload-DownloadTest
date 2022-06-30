using Microsoft.AspNetCore.Mvc;
using MongoDBUpload.Model;

namespace MongoDBUpload.Services
{
    public interface IStorageService
    {
         public void Upload(FileUploadModel model);

         public Task Download(Guid id);
    }
}
