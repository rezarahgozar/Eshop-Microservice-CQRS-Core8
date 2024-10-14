using IDP.Domain.Entities;
using IDP.Domain.IRepository.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Infra.Repository.Command
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> InsertAsync(User user)
        {
            return true;
        }
    }
}
