using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.UserConfigs
{
    public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable(nameof(UserToken));
            builder.HasOne(u => u.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(u => u.UserId);
        }
    }
}