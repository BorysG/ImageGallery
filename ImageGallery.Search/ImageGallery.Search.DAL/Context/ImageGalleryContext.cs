using ImageGallery.Search.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.Search.DAL.Context
{
    public partial class ImageGalleryContext : DbContext
    {
        public ImageGalleryContext()
        {
        }

        public ImageGalleryContext(DbContextOptions<ImageGalleryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ImageEntity> Image { get; set; }
        public virtual DbSet<ImageTagEntity> ImageTag { get; set; }
        public virtual DbSet<TagEntity> Tag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-4DIIK4U;Database=ImageGallery;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageEntity>(entity =>
            {
                entity.HasIndex(e => e.Author)
                    .HasName("Idx_Author");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Camera)
                    .HasMaxLength(100);

                entity.Property(e => e.CroppedPicture).IsRequired();

                entity.Property(e => e.ExternalId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FullPicture).IsRequired();
            });

            modelBuilder.Entity<ImageTagEntity>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.TagId })
                    .IsClustered(false);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ImageTags)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_ImageTag_Image");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ImageTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_ImageTag_Tag");
            });

            modelBuilder.Entity<TagEntity>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Idx_TagName")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
