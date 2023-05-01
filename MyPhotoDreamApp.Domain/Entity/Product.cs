using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyPhotoDreamApp.Domain.Entity
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        public int CategoryProductId { get; set; }

        [Display(Name = "Категория")]
        public CategoryProduct Category { get; set; }
    }
}
