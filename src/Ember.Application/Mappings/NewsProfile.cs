using AutoMapper;
using Ember.Application.Dto;
using Ember.Application.Features.News.Commands.Create;
using Ember.Application.Features.News.Commands.Update;
using Ember.Domain;
using Ember.Shared.Models;

namespace Ember.Application.Mappings
{
    public class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsDto>();
            CreateMap<News, GetNewsResponse>();

            CreateMap<CreateNewsCommand, News>()
                .ForMember(n => n.Id, opt => opt.Ignore());
            CreateMap<UpdateNewsCommand, News>()
                .ForMember(n => n.Id, opt => opt.Ignore());
        }
    }
}
