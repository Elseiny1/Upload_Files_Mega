using CG.Web.MegaApiClient;

namespace Upload_Files_Mega.Services.IRepo
{
    public interface IMegaService
    {
        /// <summary>
        /// Uploading Files to specific Mega Account 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Returns Node Uri</returns>
        public Task<Uri> UploadFileAsync(IFormFile file);

        /// <summary>
        /// Downloading on this server File throw its uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Full path on the server</returns>
        public Task<string> MegaDowenloadFileAsync(Uri uri);
    }
}
