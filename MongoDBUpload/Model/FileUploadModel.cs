namespace MongoDBUpload.Model
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }

        public Guid UserId { get; set; }

        public FileUploadModel()
        {

        }

        public FileUploadModel(IFormFile file, Guid userId)
        {
            using Stream stream = file.OpenReadStream();
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            File = memoryStream.ToArray();
            FileName = file.FileName;

            // var content = file.OpenReadStream().
            //File = file;//izmjeniti na end point-u
            UserId = userId;

        }
    }
}