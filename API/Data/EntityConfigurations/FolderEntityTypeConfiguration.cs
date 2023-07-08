namespace DirectoryHierarchy.Data.EntityConfigurations
{
    public class FolderEntityTypeConfiguration : IEntityTypeConfiguration<FolderEntity>
    {
        public void Configure(EntityTypeBuilder<FolderEntity> builder)
        {
            builder.ToTable("Folder");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .UseHiLo("folder_hilo")
                .IsRequired();

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(f => f.SubFolders)
                .WithOne(sb => sb.Parent)
                .HasForeignKey(f => f.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Parent)
                .WithMany(p => p.SubFolders)
                .HasForeignKey(f => f.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
