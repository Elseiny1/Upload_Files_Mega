namespace Upload_Files_Mega.Services.IRepo
{
    public interface IMegaService
    {
        public Task<Uri> UploadFileAsync(IFormFile file);
    }
}
