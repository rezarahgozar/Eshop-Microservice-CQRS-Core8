using IDP.Domain.Entities;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Infra.Repository.Query
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly ShopQueryDbContext _context;

        public UserQueryRepository(ShopQueryDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string mobileNumber)
        {
            var userFound = await _context.Tbl_Users.FirstOrDefaultAsync(q => q.MobileNumber == mobileNumber);
            return userFound;
        }
    }
}
