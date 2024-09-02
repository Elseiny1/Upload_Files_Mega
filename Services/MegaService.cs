using CG.Web.MegaApiClient;
using System.Net.Http.Headers;
using System.Text;

namespace Upload_Files_Mega.Services
{
    public class MegaService:IMegaService
    {
        private readonly HttpClient _httpClient;

        public MegaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Uri> UploadFileAsync(IFormFile file)
        {
            // Create a MegaApiClient instance and log in
            MegaApiClient client = new MegaApiClient();
            await client.LoginAsync("ahmedosama211@gmail.com", "ahe041445260");

            // Get the nodes and create a folder
            IEnumerable<INode> nodes = client.GetNodes();
            INode root = nodes.Single(x => x.Type == NodeType.Root);
            INode myFolder = client.CreateFolder("Test", root);

            // Save the uploaded file to a temporary location
            string tempFilePath = Path.GetTempFileName(); // Generates a temp file path
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Copy the IFormFile content to the temp file
            }

            try
            {
                // Upload the file to Mega using the temporary file path
                INode myFile = client.UploadFile(tempFilePath, myFolder);
                Uri downloadLink = await client.GetDownloadLinkAsync(myFile);

                await client.LogoutAsync();
                return downloadLink;
            }
            finally
            {
                // Clean up: delete the temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

    }
}
