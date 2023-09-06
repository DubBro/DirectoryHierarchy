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
            var folderNames = path.Trim(new[] { ' ', '/' }).Split("/");

            var folder = await _folderRepository.GetFolderByNameAsync(folderNames[0]);

            if (folder == null)
            {
                throw new FolderNotFoundException($"Invalid Request. Folder `{folderNames[0]}` does not exist.");
            }

            for (int i = 1; i < folderNames.Length; i++)
            {
                bool checksum = false;

                foreach (var subfolder in folder!.SubFolders)
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
                    throw new FolderNotFoundException($"Invalid Request. Folder `{folderNames[i]}` does not exist.");
                }
            }

            _logger.LogInformation($"Folder with name '{folder!.Name}' by path '{path}' was received.");

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

        public async Task<IEnumerable<FolderDTO>> GetRootFoldersAsync()
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
    }
}
