﻿using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPhotoDreamApp.Domain.ViewModels.Order
{
    public class UploadFilesViewModel
    {
        public MyPhotoDreamApp.Domain.Entity.Product Product { get; set; }
        public IFormFileCollection uploads { get; set; }
        public List<int> idInputs { get; set; } = new List<int>();
    }
}
