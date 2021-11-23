using Ember.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.Data.EntityTypeConfigurations
{
    public class AccrualConfiguration : IEntityTypeConfiguration<Accrual>
    {
        public void Configure(EntityTypeBuilder<Accrual> builder)
        {
            builder.HasKey(accrual => accrual.Id);

            builder.Property(accrual => accrual.Amount).HasColumnType("Money");
            builder.Property(accrual => accrual.Date);

            builder.HasOne(accrual => accrual.Account)
                .WithMany(account => account.Accruals);
        }
    }
}
