namespace DirectoryHierarchy.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly ILogger<FolderService> _logger;

        public FolderService(IFolderRepository folderRepository, ILogger<FolderService> logger)
        {
            _folderRepository = folderRepository;
            _logger = logger;
        }

        public async Task<FolderDTO> GetFolderAsync(string path)
        {
            try
            {
                var folderNames = path.Split("/");

                var folder = await _folderRepository.GetFolderByNameAsync(folderNames[0]);

                for (int i = 1; i < folderNames.Length; i++)
                {
                    bool checksum = false;

                    foreach (var subfolder in folder.SubFolders)
                    {
                        if (folderNames[i] == subfolder.Name)
                        {
                            folder = await _folderRepository.GetFolderByNameAsync(folderNames[i], folder.Id);
                            checksum = true;
                            break;
                        }
                    }

                    if (!checksum)
                    {
                        throw new Exception("Invalid Request. Folder does not exist.");
                    }
                }

                _logger.LogInformation($"Folder with name '{folder.Name}' by path '{path}' was received.");

                var subfolders = new List<FolderDTO>();

                foreach (var subfolder in folder.SubFolders)
                {
                    subfolders.Add(new FolderDTO
                    {
                        Id = subfolder.Id,
                        Name = subfolder.Name,
                        ParentId = subfolder.ParentId,
                    });
                }

                return new FolderDTO
                {
                    Id = folder.Id,
                    Name = folder.Name,
                    ParentId = folder.ParentId,
                    SubFolders = subfolders,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while getting a folder by path '{path}'.");
            }

            return default!;
        }

        public async Task<IEnumerable<FolderDTO>> GetRootFoldersAsync()
        {
            try
            {
                var data = await _folderRepository.GetRootFoldersAsync();

                _logger.LogInformation($"Root folders (count: {data.Count()}) were received.");

                var result = new List<FolderDTO>();

                foreach (var folder in data)
                {
                    result.Add(new FolderDTO
                    {
                        Id = folder.Id,
                        Name = folder.Name,
                        ParentId = folder.ParentId,
                        SubFolders = null!,
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting root folders.");
            }

            return default!;
        }
    }
}
