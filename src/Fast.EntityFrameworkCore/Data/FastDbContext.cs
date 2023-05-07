using System.Security.Claims;
using Fast.Infrastructure.Entities;
using Fast.Snowflake;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fast.EntityFrameworkCore.Data;

public class FastDbContext : DbContext
{
    private readonly IGeneratorSnowflakeId _generatorSnowflakeId = null!;
    private readonly IHttpContextAccessor _httpContextAccessor = null!;

    // 用于迁移
    protected FastDbContext()
    {
    }

    protected FastDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor,
        IGeneratorSnowflakeId generatorSnowflakeId) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _generatorSnowflakeId = generatorSnowflakeId;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entityEntries = ChangeTracker.Entries<AuditedEntity>().ToList();
        ConfigureAudited(entityEntries);

        return base.SaveChangesAsync(cancellationToken);
    }

    private void ConfigureAudited(List<EntityEntry<AuditedEntity>> entityEntries)
    {
        if (!entityEntries.Any())
        {
            return;
        }

        var id = GetIdByToken();

        foreach (var entityEntry in entityEntries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    ConfigureAuditedInsert(entityEntry, id);
                    break;
                case EntityState.Modified:
                    ConfigureAuditedUpdate(entityEntry, id);
                    break;
                case EntityState.Deleted:
                    ConfigureAuditedDelete(entityEntry, id);
                    break;
            }
        }
    }

    private long GetIdByToken()
    {
        var idClaim = _httpContextAccessor.HttpContext.User.FindFirst(nameof(Entity.Id));
        if (idClaim == null)
        {
            throw new Exception($"{nameof(Claim)}缺少{nameof(Entity.Id)}");
        }

        var tryParseResult = long.TryParse(idClaim.Value, out var id);
        if (!tryParseResult)
        {
            throw new Exception($"{nameof(Entity.Id)}不正确");
        }

        return id;
    }

    private void ConfigureAuditedUpdate(EntityEntry<AuditedEntity> entityEntry, long id)
    {
        entityEntry.Entity.UpdateTime = DateTime.Now;
        entityEntry.Entity.UpdateId = id;
    }

    private void ConfigureAuditedDelete(EntityEntry<AuditedEntity> entityEntry, long id)
    {
        entityEntry.State = EntityState.Modified;
        entityEntry.Entity.DeleteTime = DateTime.Now;
        entityEntry.Entity.DeleteId = id;
        entityEntry.Entity.IsEnable = false;
    }

    private void ConfigureAuditedInsert(EntityEntry<AuditedEntity> entityEntry, long id)
    {
        if (entityEntry.Entity.Id == default)
        {
            entityEntry.Entity.Id = _generatorSnowflakeId.GenerateId();
        }

        entityEntry.Entity.CreateTime = DateTime.Now;
        entityEntry.Entity.CreateId = id;
        entityEntry.Entity.IsEnable = true;
    }
}