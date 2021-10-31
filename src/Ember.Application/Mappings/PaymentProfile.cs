using AutoMapper;
using Ember.Domain;
using Ember.Shared;

namespace Ember.Application.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>();
        }
    }
}
