using Mapster;
using MediaManager.Domain.Model;
using MediaManager.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace MediaManager.WPF.Maps
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Volume, VolumeItemViewModel>
                .NewConfig();
            // .TwoWays()
            TypeAdapterConfig<M3uFile, M3uFileViewModel>
                .NewConfig()
                .TwoWays();
            return services;
        }
    }
}
