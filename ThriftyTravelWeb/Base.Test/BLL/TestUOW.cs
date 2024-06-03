using AutoMapper;
using Base.Test.DAL;

namespace Base.Test.BLL;

public class TestUOW : TestBaseUnitOfWork<TestDbContext>
{

    private readonly IMapper _mapper;

    public TestUOW(TestDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
}