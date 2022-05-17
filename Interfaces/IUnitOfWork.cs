using DataAccessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IItemRepository ItemRepository { get; }
        ICategoryRepository CategoryRepository { get; }
    }
}
