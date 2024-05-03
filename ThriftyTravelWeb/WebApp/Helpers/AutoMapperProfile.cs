using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Trip, App.DTO.v1_0.Trip>().ReverseMap();
    }
}