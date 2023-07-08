namespace MVC.Services.Interfaces
{
    public interface IFolderService
    {
        public Task<Folder> GetFolderAsync(string? path);
    }
}
