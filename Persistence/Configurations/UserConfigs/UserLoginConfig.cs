using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.UserConfigs
{
    public class UserLoginConfig : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(nameof(UserLogin));
            builder.HasOne(u => u.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(u => u.UserId);
        }
    }
}