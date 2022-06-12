// Copyrights(c) Charqe.io. All rights reserved.

using AutoMapper;
using Domain.Entities;
using PresentationLayer.ViewModels;

namespace PresentationLayer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, AppUser>().ReverseMap();
        }
    }
}