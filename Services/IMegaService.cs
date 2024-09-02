namespace Upload_Files_Mega.Services
{
    public interface IMegaService
    {
        public Task<string> AuthenticateAsync(string email, string password);
        public Task<Uri> UploadFileAsync(IFormFile file);
    }
}
