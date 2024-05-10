using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Trip, App.DTO.v1_0.Trip>().ReverseMap();
        CreateMap<App.BLL.DTO.Category, App.DTO.v1_0.Category>().ReverseMap();
        CreateMap<App.BLL.DTO.Comment, App.DTO.v1_0.Comment>().ReverseMap();
        CreateMap<App.BLL.DTO.Country, App.DTO.v1_0.Country>().ReverseMap();
        CreateMap<App.BLL.DTO.Expense, App.DTO.v1_0.Expense>().ReverseMap();
        CreateMap<App.BLL.DTO.Like, App.DTO.v1_0.Like>().ReverseMap();
        CreateMap<App.BLL.DTO.Location, App.DTO.v1_0.Location>().ReverseMap();
        CreateMap<App.BLL.DTO.Photo, App.DTO.v1_0.Photo>().ReverseMap();
        CreateMap<App.BLL.DTO.TripCategory, App.DTO.v1_0.TripCategory>().ReverseMap();
        CreateMap<App.BLL.DTO.TripLocation, App.DTO.v1_0.TripLocation>().ReverseMap();
        CreateMap<App.BLL.DTO.TripUser, App.DTO.v1_0.TripUser>().ReverseMap();
        CreateMap<App.BLL.DTO.UserExpense, App.DTO.v1_0.UserExpense>().ReverseMap();
    }
}