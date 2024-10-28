using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Upload_Files_Mega.Services.IRepo;
using Upload_Files_Mega.ViewModel;

namespace Upload_Files_Mega.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMegaService _megaService;

        public HomeController(IMegaService megaService)
        {
            _megaService = megaService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(ImagesUpload file)
        {

            var upload = await _megaService.UploadFileAsync(file.MyFile);

            if (upload == null)
                return BadRequest("Something went wrong");

            return Ok(upload);
        }

        [HttpGet("Get File")]
        public async Task<IActionResult> DownloadFileAsync(Uri uri)
        {
            var getFile= await _megaService.MegaDowenloadFileAsync(uri);
            if (getFile == null)
                return BadRequest("File Not Found");

            return Ok(getFile);

        }

        [HttpGet("Get File Name")]
        public async Task<IActionResult> GetFileNameAsync(string fileName, string folderType)
        {
            if (fileName == null || folderType == null)
                return BadRequest();

            var goalFileName = await _megaService.MegaGetFileAsync(fileName, folderType);
            return Ok(goalFileName);
        }


    }
}
