using Meiryou.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Core.Data;

public class MeiryouDbContext : DbContext
{
    public MeiryouDbContext(DbContextOptions<MeiryouDbContext> options) : base(options) { }
    
    public DbSet<ReadingContent> ReadingContents { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<ReadingContentWord> ReadingContentWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReadingContent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(2048); // Some webnovels have REALLY long titles. Should a limit exist?
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(64); // Note sure what the longest word in Japanese is, but this should be enough.
            entity.Property(e => e.Definition).HasMaxLength(-1);
            entity.Property(e => e.PartOfSpeech).HasMaxLength(64);
            entity.HasIndex(e => e.Text).IsUnique();
        });

        // Junction table.
        modelBuilder.Entity<ReadingContentWord>(entity =>
        {
            entity.HasKey(e => new { e.ReadingContentId, e.WordId });

            entity.HasOne(rcw => rcw.ReadingContent)
                .WithMany(rc => rc.ReadingContentWords)
                .HasForeignKey(rcw => rcw.ReadingContentId);

            entity.HasOne(rcw => rcw.Word)
                .WithMany(w => w.ReadingContentWords)
                .HasForeignKey(rcw => rcw.WordId);
        });
    }
}