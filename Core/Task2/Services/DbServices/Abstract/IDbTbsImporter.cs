﻿using Core.Task2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Task2.Services.DbServices.Abstract
{
    public interface IDbTbsImporter
    {
        Task<FileName> ImportAsync(string filePath);
    }
}
