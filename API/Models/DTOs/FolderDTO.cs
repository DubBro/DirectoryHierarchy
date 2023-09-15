namespace DirectoryHierarchy.Models.DTOs
{
    public class FolderDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }

        public IEnumerable<FolderDTO> SubFolders { get; set; } = new List<FolderDTO>();
    }
}
