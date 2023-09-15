namespace MVC.Services
{
    public class FolderService : IFolderService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<FolderService> _logger;

        public FolderService(IHttpClientService httpClient, ILogger<FolderService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings;
        }

        public async Task<Folder> GetFolderAsync(string? path)
        {
            Folder result;

            if (string.IsNullOrEmpty(path))
            {
                result = await _httpClient.SendAsync<Folder, string?>($"{_settings.Value.ApiUrl}", HttpMethod.Get, null);
            }
            else
            {
                result = await _httpClient.SendAsync<Folder, string?>($"{_settings.Value.ApiUrl}/?path={path}", HttpMethod.Get, null);
            }

            if (result == null)
            {
                throw new FolderNotFoundException("Result is null.");
            }

            _logger.LogInformation($"Folder '{result.Name}' with subfolders (count: {result.SubFolders.Count()}) was received.");

            return result;
        }

        public async Task CreateFolderAsync(string? path, string folderName)
        {
            if (string.IsNullOrEmpty(folderName) || folderName.Contains('/') || folderName.Length > 255)
            {
                throw new ArgumentException("Invalid folder's name.");
            }

            var currentFolder = await GetFolderAsync(path);

            foreach (var subfolder in currentFolder.SubFolders)
            {
                if (folderName == subfolder.Name)
                {
                    throw new ArgumentException("Invalid folder's name.");
                }
            }

            Folder folder = new () { Name = folderName, ParentId = currentFolder.Id == 0 ? null : currentFolder.Id };

            var result = await _httpClient.SendAsync<int, Folder>($"{_settings.Value.ApiUrl}", HttpMethod.Post, folder);

            if (result == 0)
            {
                throw new BussinessException("Error occurred while creating folder.");
            }

            _logger.LogInformation($"Folder '{folderName}' was created with id = {result}.");
        }
    }
}
