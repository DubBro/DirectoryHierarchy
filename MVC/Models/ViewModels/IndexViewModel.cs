namespace MVC.Models.ViewModels
{
    public class IndexViewModel
    {
        public string Title { get; set; } = null!;

        public IEnumerable<SubFolderViewModel> SubFolders { get; set; } = null!;
    }
}
