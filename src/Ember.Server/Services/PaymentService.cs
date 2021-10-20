using Ember.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ember.Server.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext context;

        public PaymentService(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void ToPay()
        {
            
        }
    }
}
