using AutoMapper;
using DataAccessAPI.Data;
using DataAccessAPI.Repositories;


namespace DataAccessAPI.Interfaces
{
    public class UnitOfWorkEF : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWorkEF(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IItemRepository ItemRepository => new ItemRepositoryEF(_context,_mapper);

        public ICategoryRepository CategoryRepository => new CategoryRepositoryEF(_context, _mapper);

    }
}
