using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nerosoft.Starfish.Repository;

/// <summary>
/// The entity configuration for <see cref="DictionaryRoot"/>.
/// </summary>
[DbContext(typeof(SupportDataContext))]
internal sealed class DictionaryRootEntityConfiguration : IEntityTypeConfiguration<DictionaryRoot>
{
    private const string TABLE = "dictionary_root";

    public void Configure(EntityTypeBuilder<DictionaryRoot> builder)
    {
        builder.ToTable(TABLE);

        builder.HasKey(t => t.Id);
        builder.HasIndex(t => t.Code)
               .IsUnique()
               .HasDatabaseName($"{TABLE}_idx_code");

        builder.SnowflakeId();

        builder.Property(t => t.Code)
               .HasColumnName("code")
               .HasMaxLength(128)
               .IsRequired()
               .IsUnicode();

        builder.Property(t => t.Name)
               .HasColumnName("name")
               .HasMaxLength(255)
               .IsRequired()
               .IsUnicode();

        builder.Property(t => t.Remark)
               .HasColumnName("remark")
               .HasMaxLength(1000)
               .IsUnicode();

        builder.Property(t => t.IsValid)
               .HasColumnName("is_valid")
               .HasDefaultValue(true);

        builder.ConfigureAuditableProperties();

        builder.HasMany(t => t.Items)
               .WithOne(t => t.Root)
               .HasForeignKey(t => t.Root)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
