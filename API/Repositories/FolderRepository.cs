namespace DirectoryHierarchy.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FolderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FolderEntity>> GetRootFoldersAsync()
        {
             return await _dbContext.Folders.Where(f => f.ParentId == null).Include(f => f.SubFolders).ToListAsync();
        }

        public async Task<FolderEntity> GetFolderByNameAsync(string name, int? parentId = null)
        {
            return await _dbContext.Folders.Where(f => f.Name == name && f.ParentId == parentId).Include(f => f.SubFolders).SingleAsync();
        }
    }
}
