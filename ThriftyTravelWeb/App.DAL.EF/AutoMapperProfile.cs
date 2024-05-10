using AutoMapper;
using Domain.Entities;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Category, DAL.DTO.Category>().ReverseMap();
        CreateMap<Comment, DAL.DTO.Comment>().ReverseMap();
        CreateMap<Country, DAL.DTO.Country>().ReverseMap();
        CreateMap<Expense, DAL.DTO.Expense>().ReverseMap();
        CreateMap<Like, DAL.DTO.Like>().ReverseMap();
        CreateMap<Location, DAL.DTO.Location>().ReverseMap();
        CreateMap<Photo, DAL.DTO.Photo>().ReverseMap();
        CreateMap<Trip, DAL.DTO.Trip>().ReverseMap();
        CreateMap<TripCategory, DAL.DTO.TripCategory>().ReverseMap();
        CreateMap<TripLocation, DAL.DTO.TripLocation>().ReverseMap();
        CreateMap<TripUser, DAL.DTO.TripUser>().ReverseMap();
        CreateMap<UserExpense, DAL.DTO.UserExpense>().ReverseMap();
    }
}