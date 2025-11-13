using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerosoft.Euonia.Domain;
using Nerosoft.Euonia.Repository.EfCore;

namespace Nerosoft.Starfish.Repository;

internal static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Configure auditable properties for entity type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="builder"></param>
    public static void ConfigureAuditableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IAuditing
    {
        builder.Property(t => t.CreateTime)
               .HasColumnName("create_time")
               .HasValueGenerator<UtcTimeValueGenerator>()
               .ValueGeneratedOnAdd()
               .IsRequired();

        builder.Property(t => t.UpdateTime)
               .HasColumnName("update_time")
               .HasValueGenerator<UtcTimeValueGenerator>()
               .ValueGeneratedOnAddOrUpdate()
               .IsRequired();

        builder.Property(t => t.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(64)
               .IsRequired()
               .IsUnicode();

        builder.Property(t => t.UpdatedBy)
               .HasColumnName("updated_by")
               .HasMaxLength(64)
               .IsRequired()
               .IsUnicode();
    }

    /// <summary>
    /// Configure tombstone properties for entity type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="builder"></param>
    public static void ConfigureTombstoneProperty<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ITombstone
    {
        builder.Property(t => t.IsDeleted)
               .HasColumnName("is_deleted")
               .HasDefaultValue(false)
               .IsRequired();

        builder.Property(t => t.DeleteTime)
               .HasColumnName("delete_time");
    }

    /// <summary>
    /// Configure Snowflake ID property for entity type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="builder"></param>
    public static void SnowflakeId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity<long>
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
               .HasColumnName("id")
               .IsRequired()
               .HasValueGenerator<SnowflakeIdValueGenerator>()
               .ValueGeneratedOnAdd();
    }
}
