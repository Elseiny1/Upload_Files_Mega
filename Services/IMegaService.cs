namespace Upload_Files_Mega.Services
{
    public interface IMegaService
    {
        public Task<Uri> UploadFileAsync(IFormFile file);
    }
}
