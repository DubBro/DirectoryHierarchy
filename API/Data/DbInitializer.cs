namespace DirectoryHierarchy.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Folders.Any())
            {
                await context.Folders.AddRangeAsync(
                    new List<FolderEntity>()
                    {
                        new FolderEntity() { Name = "Creating Digital Images", ParentId = null },
                        new FolderEntity() { Name = "Resources", ParentId = 1 },
                        new FolderEntity() { Name = "Evidence", ParentId = 1 },
                        new FolderEntity() { Name = "Graphic Products", ParentId = 1 },
                        new FolderEntity() { Name = "Primary Sources", ParentId = 2 },
                        new FolderEntity() { Name = "Secondary Sources", ParentId = 2 },
                        new FolderEntity() { Name = "Process", ParentId = 4 },
                        new FolderEntity() { Name = "Final Product", ParentId = 4 },
                    });

                await context.SaveChangesAsync();
            }
        }
    }
}
