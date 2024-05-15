using AutoMapper;
using Celestin.API.CelestinClassifier;
using Celestin.API.Common;
using Celestin.API.Models.CelestinModels;
using System;
using System.Linq.Expressions;

namespace Celestin.API.Profiles
{
    public class CelestinProfile : Profile
    {
        public CelestinProfile()
        {
            CreateMap<DbModels.Celestin, CelestinWithDiscoveryDto>()
            .ForMember(dest => dest.DiscoveryName, opt => opt.MapFrom(src => src.DiscoverySource.Name))
             .ForMember(dest => dest.ObjectType, opt => opt.MapFrom(src => GetCelestinType(src)));

            CreateMap<DbModels.Celestin, CelestinWithDiscoveryObjectDto>()
                  .ForMember(dest => dest.Discovery, opt => opt.MapFrom(src => src.DiscoverySource));

            CreateMap<DbModels.Celestin, CelestinWithoutDiscoveryDto>();

            CreateMap<DbModels.DiscoverySource, Models.DiscoverySourceDto>()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name));

            CreateMap<CelestinForCreationDto, DbModels.Celestin>();
        }

        private string GetCelestinType(DbModels.Celestin item)
        {
            var itemTypes = string.Empty;
            var massLimit = Commons.massOfJupiter * 13;
            var radius = item.EquatorialDiameter / 2;
            double schwarzschildRadius = (2 * Commons.gravitationalConstant * item.Mass) / Math.Pow(Commons.speedOfLight, 2);
            var hasOneTypeAtLeast = false;

            if (item.Mass < massLimit)
            {
                itemTypes += typeof(Planets).Name + " ";
                hasOneTypeAtLeast = true;
            }

            if (item.Mass >= massLimit && item.SurfaceTemperature >= 2500)
            {
                itemTypes += typeof(Stars).Name + " ";
                hasOneTypeAtLeast = true;
            }

            if (radius < schwarzschildRadius)
            {
                itemTypes += typeof(BlackHoles).Name + " ";
                hasOneTypeAtLeast = true;
            }

            if (!hasOneTypeAtLeast)
            {
                itemTypes = Commons.unknown;
            }

            return itemTypes;
        }
    }
}
