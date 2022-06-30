using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDBUpload.Model;
using MongoDBUpload.Services;

namespace MongoDBUpload.Services
{
    public class StorageService : IStorageService
    {
        private readonly string ConnectionString = "mongodb://127.0.0.1:27017"; //conn string for mongo db base
        private readonly string DatabaseName = "file_storage_db";
        private readonly IMongoCollection<FileUploadModel> FileCollection;

        public StorageService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            //var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);

            //var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

        }
        public void Upload(FileUploadModel model)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);

            Guid id = new Guid();
            IGridFSBucket<Guid> bucket = new GridFSBucket<Guid>(db);


            //bucket.Upload(model.file.ToBson())

            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument()
                {
                 {"author", "TestAuthor"},

                 {"time", DateTime.UtcNow}
                 }
            };

            // string fileName = model.file.FileName;
            try
            {
                //Nece da upise kada se salje isti ID koji je vec upisan
                bucket.UploadFromBytes(model.UserId, model.FileName, model.File, options);
            }
            catch (Exception e)
            {
                throw new Exception("File isn't uploaded");
            }


            Console.WriteLine(id.ToString() + " whatever");
        }


        /*  -Attention-
        *
        *   I just try to update code and try to download with filter which include GUID id.
        *
        *   You can try to do same with ObjectID.
        *   
        *   It was working when I try to filter with file name.
        */
        public async Task Download(Guid id)
        {
            
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);

            IGridFSBucket<Guid> bucket = new GridFSBucket<Guid>(db);

            // filter the db and find the file which is specified by that filter, you can pass ID or something else instead filename.
           var filter = Builders<GridFSFileInfo<Guid>>.Filter.Eq(id.ToString(), "6a1a8895-9b2e-4702-b1ad-61d33b4599d7");

            //Console.WriteLine("GUID TO STRING : " + filter);

            var searchResult = await bucket.FindAsync(filter); //ovde ga ne pronadje kako treba

            var fileEntry = searchResult.FirstOrDefault();

            Console.WriteLine("file entry ID je : " + fileEntry.Id.ToString());

         
            var returnModel = new FileUploadModel();

            returnModel.File = bucket.DownloadAsBytes(fileEntry.Id);
            returnModel.FileName = fileEntry.Filename;

            /*using (Stream fs = new FileStream(file, FileMode.CreateNew, FileAccess.Write))
            {
                await bucket.DownloadToStreamAsync(fileEntry.Id, fs);
                fs.Close();
            }*/


    }
}
}
