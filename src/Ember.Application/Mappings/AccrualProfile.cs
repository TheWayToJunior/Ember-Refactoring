using AutoMapper;
using Ember.Domain;
using Ember.Shared;

namespace Ember.Application.Mappings
{
    public class AccrualProfile : Profile
    {
        public AccrualProfile()
        {
            CreateMap<Accrual, AccrualDTO>();
        }
    }
}
