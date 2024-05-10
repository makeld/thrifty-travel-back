using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DAL.DTO.Category, App.BLL.DTO.Category>().ReverseMap();
        CreateMap<DAL.DTO.Comment, App.BLL.DTO.Comment>().ReverseMap();
        CreateMap<DAL.DTO.Country, App.BLL.DTO.Country>().ReverseMap();
        CreateMap<DAL.DTO.Expense, App.BLL.DTO.Expense>().ReverseMap();
        CreateMap<DAL.DTO.Like, App.BLL.DTO.Like>().ReverseMap();
        CreateMap<DAL.DTO.Location, App.BLL.DTO.Location>().ReverseMap();
        CreateMap<DAL.DTO.Photo, App.BLL.DTO.Photo>().ReverseMap();
        CreateMap<DAL.DTO.Trip, App.BLL.DTO.Trip>().ReverseMap();
        CreateMap<DAL.DTO.TripCategory, App.BLL.DTO.TripCategory>().ReverseMap();
        CreateMap<DAL.DTO.TripLocation, App.BLL.DTO.TripLocation>().ReverseMap();
        CreateMap<DAL.DTO.TripUser, App.BLL.DTO.TripUser>().ReverseMap();
        CreateMap<DAL.DTO.UserExpense, App.BLL.DTO.UserExpense>().ReverseMap();
    }
}