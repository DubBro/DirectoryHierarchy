namespace DirectoryHierarchy.Data.Entities
{
    public class FolderEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public FolderEntity? Parent { get; set; }

        public int? ParentId { get; set; }

        public IEnumerable<FolderEntity> SubFolders { get; set; } = new List<FolderEntity>();
    }
}
