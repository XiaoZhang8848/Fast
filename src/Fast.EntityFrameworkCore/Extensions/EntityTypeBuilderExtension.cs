using System.Linq.Expressions;
using Fast.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fast.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtension
{
    public static void Configure<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : Entity
    {
        builder.Property(nameof(Entity.Id)).ValueGeneratedNever();

        if (builder.Metadata.ClrType.IsAssignableTo(typeof(AuditedEntity)))
        {
            ConfigureAudited(builder);
        }
    }

    private static void ConfigureAudited<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : Entity
    {
        Expression<Func<TEntity, bool>> expression = x => EF.Property<bool>(x, nameof(AuditedEntity.IsEnable));
        builder.HasQueryFilter(expression);

        builder.Property(nameof(AuditedEntity.CreateTime));
        builder.Property(nameof(AuditedEntity.CreateId));
        builder.Property(nameof(AuditedEntity.UpdateTime));
        builder.Property(nameof(AuditedEntity.UpdateId));
        builder.Property(nameof(AuditedEntity.DeleteTime));
        builder.Property(nameof(AuditedEntity.DeleteId));
        builder.Property(nameof(AuditedEntity.IsEnable));
    }
}