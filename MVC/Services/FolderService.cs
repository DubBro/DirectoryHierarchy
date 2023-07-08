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

            if (path == null)
            {
                result = await _httpClient.SendAsync<Folder, string?>($"{_settings.Value.ApiUrl}", HttpMethod.Get, null);
            }
            else
            {
                result = await _httpClient.SendAsync<Folder, string?>($"{_settings.Value.ApiUrl}/?path={path}", HttpMethod.Get, null);
            }

            if (result != null)
            {
                _logger.LogInformation($"Folder '{result.Name}' with subfolders (count: {result.SubFolders.Count()}) was received.");
            }
            else
            {
                _logger.LogInformation($"Result is null.");
            }

            return result!;
        }
    }
}
