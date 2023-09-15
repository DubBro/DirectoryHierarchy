namespace MVC.Models
{
    public class Folder
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }

        public IEnumerable<Folder> SubFolders { get; set; } = new List<Folder>();
    }
}
