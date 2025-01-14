using IDP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.IRepository.Query
{
    public interface IUserQueryRepository
    {
        Task<User> GetUserAsync(String mobileNumber);
    }
}
