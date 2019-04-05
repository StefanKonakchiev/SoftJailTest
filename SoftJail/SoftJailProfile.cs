namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<ImportOfficersDto, Officer>()
                .ForMember(x => x.Position, y => y.MapFrom(s => Enum.Parse<Position>(s.Position)))
                .ForMember(x => x.Weapon, y => y.MapFrom(s => Enum.Parse<Weapon>(s.Weapon)));
        }
    }
}
