using CG.Web.MegaApiClient;
using System.Net.Http.Headers;
using System.Text;
using Upload_Files_Mega.Services.IRepo;

namespace Upload_Files_Mega.Services.Ropo
{
    public class MegaService : IMegaService
    {
        private readonly HttpClient _httpClient;
        private static string tempFilePath;

        public MegaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //here is all private methodes to serve on action methods
        #region Upload Files

        private async Task<INode> GetMegaNodesAsync(MegaApiClient client)
        {
            // Get the nodes and create a folder
            IEnumerable<INode> nodes = client.GetNodes();
            INode root = nodes.Single(x => x.Type == NodeType.Root);
            INode myFolder = client.CreateFolder("Test", root);

            return myFolder;
        }

        private async Task<string> GetTemporaryFileAsync(IFormFile file)
        {
            // Save the uploaded file to a temporary location
            tempFilePath = Path.GetTempFileName(); // Generates a temp file path
            string FileName = file.FileName;
            string fileExtenstion = Path.GetExtension(FileName);
            string FilePath = Path.ChangeExtension(tempFilePath, fileExtenstion);
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Copy the IFormFile content to the temp file
            }

            return FilePath;
        }
        #endregion



        public async Task<Uri> UploadFileAsync(IFormFile file)
        {
            if (file == null)
                return null;

            MegaApiClient client = new MegaApiClient();
            try
            {
                // Create a MegaApiClient instance and log in
                await client.LoginAsync("ahmedosama211@gmail.com", "ahe041445260");
            }
            catch (Exception ex)
            {
                return null;
            }

            var myFolder = await GetMegaNodesAsync(client);

            var FilePath = await GetTemporaryFileAsync(file);

            try
            {
                // Upload the file to Mega using the temporary file path
                INode myFile = client.UploadFile(FilePath, myFolder);
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
