using DirectoryHierarchy.Data.EntityConfigurations;

namespace DirectoryHierarchy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FolderEntity> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FolderEntityTypeConfiguration());
        }
    }
}
