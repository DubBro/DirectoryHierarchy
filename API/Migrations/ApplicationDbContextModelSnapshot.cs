using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace DirectoryHierarchy.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    public partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("folder_hilo")
                .IncrementsBy(10);

            modelBuilder.Entity("DirectoryHierarchy.Data.Entities.FolderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "folder_hilo");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Folder", (string)null);
                });

            modelBuilder.Entity("DirectoryHierarchy.Data.Entities.FolderEntity", b =>
                {
                    b.HasOne("DirectoryHierarchy.Data.Entities.FolderEntity", "Parent")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("DirectoryHierarchy.Data.Entities.FolderEntity", b =>
                {
                    b.Navigation("SubFolders");
                });
#pragma warning restore 612, 618
        }
    }
}
