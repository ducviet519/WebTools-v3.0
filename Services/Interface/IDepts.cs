﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entity;

namespace WebTools.Services.Interface
{
    public interface IDepts
    {
        public Task<List<Depts>> GetAll_DeptsAsync();
    }
}
