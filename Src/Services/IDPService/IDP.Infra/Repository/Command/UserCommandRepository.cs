﻿using IDP.Domain.Entities;
using IDP.Domain.IRepository.Command;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Infra.Repository.Command
{
    public class UserCommandRepository : CommandRepository<User>,IUserCommandRepository
    {
        public UserCommandRepository(ShopCommandDbContext context) :base(context)
        {
        }
    }
}
