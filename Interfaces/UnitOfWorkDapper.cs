using AutoMapper;
using DataAccessAPI.Repositories;
using Microsoft.Extensions.Configuration;

namespace DataAccessAPI.Interfaces
{
    public class UnitOfWorkDapper : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UnitOfWorkDapper(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IItemRepository ItemRepository => new ItemRepositoryDapper(_configuration, _mapper);

        public ICategoryRepository CategoryRepository => new CategoryRepositoryDapper(_configuration, _mapper);
    }
}
