using MyPhotoDreamApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyPhotoDreamApp.Domain.ViewModels.Home
{
    public class ProductListViewModel
    {
        public List<MyPhotoDreamApp.Domain.Entity.Product> Products { get; set; }
        
    }
}
