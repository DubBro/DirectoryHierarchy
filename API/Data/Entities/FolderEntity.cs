namespace DirectoryHierarchy.Data.Entities
{
    public class FolderEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public FolderEntity Parent { get; set; } = null!;

        public int? ParentId { get; set; } = null!;

        public IEnumerable<FolderEntity> SubFolders { get; set; } = null!;
    }
}
