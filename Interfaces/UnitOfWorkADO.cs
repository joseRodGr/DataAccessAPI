using AutoMapper;
using DataAccessAPI.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Interfaces
{
    public class UnitOfWorkADO : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UnitOfWorkADO(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IItemRepository ItemRepository => new ItemRepositoryADO(_configuration, _mapper);

        public ICategoryRepository CategoryRepository => new CategoryRepositoryADO(_configuration,_mapper);
    }
}
