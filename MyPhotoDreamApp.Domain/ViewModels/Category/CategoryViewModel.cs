using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Domain.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }


        [MaxLength(25, ErrorMessage = "Название не должно превышать длину в 25 символов")]
        [Required(ErrorMessage = "Укажите название категории")]
        [Display(Name = "Название категории")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Описание не должно превышать длину в 100 симврлов")]
        public string? Description { get; set; }
    }
}
