using Domain.Entities;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.UserConfigs;
using Utils;

namespace Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        base.SavingChanges += OnSavingChanges;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    private void OnSavingChanges(object sender, SavingChangesEventArgs e)
    {
        ConfigureEntityDates();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var entitiesAssembly = typeof(IEntity).Assembly;
        builder.RegisterAllEntities<IEntity>(entitiesAssembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        builder.ApplyGlobalFilters<bool>("IsDeleted", false);
    }

    private void ConfigureEntityDates()
    {
        var updatedEntities = ChangeTracker.Entries().Where(x =>
            x.Entity is ITimeModification && x.State == EntityState.Modified).Select(x => x.Entity as ITimeModification);

        var addedEntities = ChangeTracker.Entries().Where(x =>
            x.Entity is ITimeModification && x.State == EntityState.Added).Select(x => x.Entity as ITimeModification);

        foreach (var entity in updatedEntities)
        {
            if (entity != null)
            {
                entity.ModifiedDate = DateTime.Now;
            }
        }

        foreach (var entity in addedEntities)
        {
            if (entity != null)
            {
                entity.CreatedTime = DateTime.Now;
                entity.ModifiedDate = DateTime.Now;
            }
        }
    }
}