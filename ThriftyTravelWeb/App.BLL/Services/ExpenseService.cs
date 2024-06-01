using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class ExpenseService :
    BaseEntityService<App.DAL.DTO.Expense, App.BLL.DTO.Expense, IExpenseRepository>,
    IExpenseService
{
    private readonly ILocationRepository _locationRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ITripLocationRepository _tripLocationRepository;
    private readonly IPhotoRepository _photoRepository;
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public ExpenseService(IAppUnitOfWork uoW, IExpenseRepository repository, ICountryRepository countryRepository,  
        ILocationRepository locationRepository, ITripLocationRepository tripLocationRepository, 
        IPhotoRepository photoRepository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Expense, App.BLL.DTO.Expense>(mapper))
    {
        _locationRepository = locationRepository;
        _countryRepository = countryRepository;
        _tripLocationRepository = tripLocationRepository;
        _photoRepository = photoRepository;
        _uow = uoW;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Expense?>> GetExpenseByTripId(Guid tripId)
    {
        return (await Repository.GetExpenseByTripId(tripId)).Select(e => Mapper.Map(e));
    }
    
    public async Task<double> CalculateExpensesTotal(Guid tripId)
    {
        return await Repository.CalculateExpensesTotal(tripId);
    }
    
    public async Task<App.BLL.DTO.Expense> CreateExpenseWithAttributesAsync(App.BLL.DTO.AddExpense expenseData)
    {
        var newCountry = new Country()
        {
            Id = Guid.NewGuid(),
            Name = expenseData.CountryName,
            Continent = expenseData.CountryContinent
        };
        
        var dalCountry = _mapper.Map<App.DAL.DTO.Country>(newCountry);
        _countryRepository.Add(dalCountry);
        await _uow.SaveChangesAsync();

        var newLocation = new Location()
        {
            Id = Guid.NewGuid(),
            CountryId = dalCountry.Id,
            LocationName = expenseData.LocationName
        };
        
        var dalLocation = _mapper.Map<App.DAL.DTO.Location>(newLocation);
        _locationRepository.Add(dalLocation);
        await _uow.SaveChangesAsync();

        var tripLocation = new TripLocation()
        {
            Id = Guid.NewGuid(),
            TripId = expenseData.TripId,
            LocationId = newLocation.Id
        };
        
        var dalTripLocation = _mapper.Map<App.DAL.DTO.TripLocation>(tripLocation);
        _tripLocationRepository.Add(dalTripLocation);
        await _uow.SaveChangesAsync();
        
        var newExpense = new Expense()
        {
            Id = Guid.NewGuid(),
            TripId = expenseData.TripId,
            TripLocationId = dalTripLocation.Id,
            Title = expenseData.ExpenseTitle,
            Type = expenseData.ExpenseType,
            Description = expenseData.ExpenseDescription,
            ExpenseDate = expenseData.ExpenseDate,
            ExpensePrice = expenseData.ExpensePrice,
            CurrencyCode = expenseData.ExpenseCurrencyCode
        };
            
        var createdExpense = Add(newExpense);
        await _uow.SaveChangesAsync();
        
        if (!string.IsNullOrEmpty(expenseData.ImageUrl))
        {
            var photo = new Photo()
            {
                Id = Guid.NewGuid(),
                ExpenseId = createdExpense.Id,
                ImageUrl = expenseData.ImageUrl!,
                Description = expenseData.ImageDescription
            };
                
            var dalPhoto = _mapper.Map<App.DAL.DTO.Photo>(photo);

            _photoRepository.Add(dalPhoto);
            await _uow.SaveChangesAsync();
        }

        return createdExpense;
    }
}