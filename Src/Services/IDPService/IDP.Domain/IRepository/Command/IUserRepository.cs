using IDP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.IRepository.Command
{
    public interface IUserRepository
    {
        Task<bool> InsertAsync(User user);
    }
}
