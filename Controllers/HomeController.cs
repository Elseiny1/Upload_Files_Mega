using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Upload_Files_Mega.Services;

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
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var upload=await _megaService.UploadFileAsync(file);

            return Ok(upload);
        }

    }
}
