using Ember.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.Data.EntityTypeConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(payment => payment.Id);

            builder.Property(payment => payment.Amount).HasColumnType("Money");
            builder.Property(payment => payment.Date);

            builder.HasOne(payment => payment.Account)
                .WithMany(account => account.Payments);
        }
    }
}
