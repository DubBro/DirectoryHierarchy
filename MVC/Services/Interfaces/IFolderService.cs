namespace MVC.Services.Interfaces
{
    public interface IFolderService
    {
        Task<Folder> GetFolderAsync(string? path);
        Task CreateFolderAsync(string? path, string folderName);
    }
}
