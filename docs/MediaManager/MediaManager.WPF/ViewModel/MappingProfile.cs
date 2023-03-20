namespace MediaManager.WPF.ViewModel
{
    using AutoMapper;
    using MediaManager.Domain.Model;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Volume, VolumeItemViewModel>()
                .ReverseMap();

            CreateMap<M3uFile, M3uFileViewModel>()
                .ReverseMap();
        }
    }
}
