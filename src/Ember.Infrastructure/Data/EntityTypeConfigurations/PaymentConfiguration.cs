using Ember.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.Data.EntityTypeConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(ph => ph.Id);

            builder.Property(ph => ph.Amount);
            builder.Property(ph => ph.Date);

            builder.HasOne(ph => ph.Account)
                .WithMany(account => account.Payments);
        }
    }
}
