using IDP.Domain.Entities;
using IDP.Domain.IRepository.Command.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.IRepository.Command
{
    public interface IUserCommandRepository : ICommandRepository<User>
    {
    }
}
