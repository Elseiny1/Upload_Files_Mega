using CG.Web.MegaApiClient;
using System.IO.Pipelines;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
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
        #region Private Methodes
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

        private async Task<MegaApiClient> MegaLogInAsync(string email, string password, MegaApiClient client)
        {
            try
            {
                // Create a MegaApiClient instance and log in
                await client.LoginAsync(email, password);
            }
            catch (Exception ex)
            {
                return null;
            }
            return client;
        }

        #endregion

        #region Get Files
        private async Task<string> MegaServerDownloadAsync(Uri uri, MegaApiClient client)
        {
            INode node = client.GetNodeFromLink(uri);

            if (node == null)
                return null;

            var downloadPath = Path.GetFullPath("Files");
            var fullPath = Path.Combine(downloadPath, node.Name);

            if (File.Exists(fullPath))
                return fullPath;

            Stream stream = new FileStream(fullPath, FileMode.Create);
            stream = client.Download(node);

            return fullPath;
        }

        #endregion

        #region Action Methods

        public async Task<Uri> UploadFileAsync(IFormFile file)
        {
            if (file == null)
                return null;

            var fileSize = file.Length;

            MegaApiClient client = new MegaApiClient();
            client = await MegaLogInAsync("ahmedosama211@gmail.com", "ahe041445260", client);

            if (client == null)
                return null;

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

        public async Task<string> MegaDowenloadFileAsync(Uri uri)
        {
            if(uri == null)
                return "Invalid uri";

            MegaApiClient client = new MegaApiClient();
            client = await MegaLogInAsync("ahmedosama211@gmail.com", "ahe041445260", client);

            if (client == null)
                return "Invalid Mega account";

            var filePath = await MegaServerDownloadAsync(uri, client);

            if (filePath == null)
                return "File not Found";

            return filePath ;
        }

        public async Task<Uri> MegaGetFileAsync(string fileName, string folderType)
        {
            MegaApiClient client = new MegaApiClient();
            client = await MegaLogInAsync("ahmedosama211@gmail.com", "ahe041445260", client);

            if (client == null)
                return null;
            try
            {
                var nodes = await client.GetNodesAsync();
                var allFiles = nodes.Where(f => f.Type == NodeType.File).ToList();
                var goalFile = nodes.Where(n => n.Name == fileName).FirstOrDefault();

                if (goalFile == null)
                    return null;

                Uri uri = client.GetDownloadLink(goalFile);

                return uri;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
