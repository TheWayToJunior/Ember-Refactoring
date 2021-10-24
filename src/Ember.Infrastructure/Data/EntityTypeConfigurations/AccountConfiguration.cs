using Ember.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.Data.EntityTypeConfigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("PersonalAccounts");

            builder.HasKey(account => account.Id);
            builder.HasIndex(account => account.Id).IsUnique();

            builder.Property(account => account.Number)
                .HasMaxLength(6).ValueGeneratedNever();
            builder.HasIndex(account => account.Number).IsUnique();

            builder.Property(account => account.Payment).HasColumnType("Money");
            builder.Property(account => account.Address);

            builder.HasMany(account => account.Payments)
                .WithOne(ph => ph.Account);
        }
    }
}
