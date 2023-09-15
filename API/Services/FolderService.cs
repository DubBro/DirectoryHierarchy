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

        public async Task<FolderDTO> GetFolderAsync(string? path)
        {
            if (path == null)
            {
                var rootFolders = await GetRootFoldersAsync();

                var result = new FolderDTO()
                {
                    Id = 0,
                    Name = "Root",
                    SubFolders = rootFolders,
                    ParentId = null,
                };

                return result;
            }

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

        public async Task<int> AddFolderAsync(FolderDTO folderDTO)
        {
            await ValidateFolderDTO(folderDTO);

            FolderEntity folderEntity = new ()
            {
                Name = folderDTO.Name,
                ParentId = folderDTO.ParentId,
            };

            var result = await _folderRepository.AddFolderAsync(folderEntity);

            _logger.LogInformation($"Folder `{folderDTO.Name}` with ID = {result} was added.");

            return result;
        }

        private async Task ValidateFolderDTO(FolderDTO folderDTO)
        {
            if (folderDTO == null)
            {
                throw new ArgumentNullException(nameof(folderDTO));
            }

            if (string.IsNullOrEmpty(folderDTO.Name) || folderDTO.Name.Contains("/") || folderDTO.Name.Length > 255)
            {
                throw new ArgumentException(nameof(folderDTO.Name));
            }

            if (folderDTO.ParentId <= 0)
            {
                throw new ArgumentException(nameof(folderDTO.ParentId));
            }

            if (await _folderRepository.GetFolderByNameAsync(folderDTO.Name, folderDTO.ParentId) != null)
            {
                throw new ArgumentException(nameof(folderDTO.Name));
            }
        }
    }
}
