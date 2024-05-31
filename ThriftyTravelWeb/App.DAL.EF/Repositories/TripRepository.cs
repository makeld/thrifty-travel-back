using App.Contracts.DAL.Repositories;
using AutoMapper;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TripRepository : BaseEntityRepository<Trip, DALDTO.Trip, AppDbContext>,
    ITripRepository
{
    public TripRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<Trip, DALDTO.Trip>(mapper))
    {
    }
    
}