using AutoMapper;
using Ember.Domain;
using Ember.Shared;

namespace Ember.Application.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDTO>();
        }
    }
}
