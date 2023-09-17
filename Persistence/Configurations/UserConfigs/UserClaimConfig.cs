using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.UserConfigs
{
    public class UserClaimConfig : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(u => u.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(u => u.UserId);

            builder.ToTable(nameof(UserClaim));
        }
    }
}