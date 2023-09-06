namespace DirectoryHierarchy.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<FolderEntity>> GetRootFoldersAsync();
        Task<FolderEntity?> GetFolderByNameAsync(string name, int? parentId = null);
    }
}
