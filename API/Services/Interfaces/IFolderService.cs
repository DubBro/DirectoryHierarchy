namespace DirectoryHierarchy.Services.Interfaces
{
    public interface IFolderService
    {
        Task<FolderDTO> GetFolderAsync(string path);
        Task<IEnumerable<FolderDTO>> GetRootFoldersAsync();
    }
}
