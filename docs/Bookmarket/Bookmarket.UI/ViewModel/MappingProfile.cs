using AutoMapper;
using Bookmarket.Domain.Models;

namespace Bookmarket.UI.ViewModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bookmark, BookmarkItemViewModel>()
                .ReverseMap();
            CreateMap<Tag, TagViewModel>()
                .ReverseMap();
        }
    }
}
