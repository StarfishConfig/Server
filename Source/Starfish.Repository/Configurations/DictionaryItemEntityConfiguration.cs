using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The entity configuration for <see cref="DictionaryItem"/>.
/// </summary>
[DbContext(typeof(SystemDataContext))]
internal class DictionaryItemEntityConfiguration : IEntityTypeConfiguration<DictionaryItem>
{
    private const string TABLE = "dictionary_item";

    public void Configure(EntityTypeBuilder<DictionaryItem> builder)
    {
        builder.ToTable(TABLE);

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.RootId)
               .HasDatabaseName($"{TABLE}_idx_root_id");

        builder.HasIndex(t => new { t.RootId, t.Key })
               .IsUnique()
               .HasDatabaseName($"{TABLE}_idx_unique");

        builder.SnowflakeId();

        builder.Property(t => t.RootId)
               .HasColumnName("root_id")
               .IsRequired();

        builder.Property(t => t.Key)
               .HasColumnName("key")
               .HasMaxLength(128)
               .IsRequired()
               .IsUnicode();

        builder.Property(t => t.Value)
               .HasColumnName("value")
               .HasMaxLength(255)
               .IsRequired()
               .IsUnicode();

        builder.Property(t => t.Remark)
               .HasColumnName("remark")
               .HasMaxLength(500)
               .IsRequired()
               .IsUnicode();

        builder.HasOne(t => t.Root)
               .WithMany(t => t.Items)
               .HasForeignKey(t => t.RootId);
    }
}