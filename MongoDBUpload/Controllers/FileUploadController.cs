using Microsoft.AspNetCore.Mvc;
using MongoDBUpload.Model;
using MongoDBUpload.Services;

namespace MongoDBUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase  
    {
        private readonly IStorageService _service;

        public FileUploadController(IStorageService service)
        {
            _service = service;
        }

        public IActionResult Get()
        {
            return Ok("File Upload API running");
        }

        // you have to pass here ObjectID (of FileID) through Swagger or something else what you need to be passed.
        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> Upload(IFormFile file, [FromForm]Guid userId)
        {
            var returnModel = new FileUploadModel(file, userId);
          //  _service.Upload(returnModel);

            // you have to pass here ObjectID (of FileID) through Swagger or something else what you need to be passed.
           await _service.Download(returnModel.UserId);
            return Ok();
        }
    }
}
