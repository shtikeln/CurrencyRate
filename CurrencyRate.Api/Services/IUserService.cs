﻿using CurrencyRate.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.Api.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}
