using MyPhotoDreamApp.Domain.Entity;
using MyPhotoDreamApp.Domain.Response;
using MyPhotoDreamApp.Domain.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDreamApp.Service.Interfaces
{
    public interface ICategoryProductService
    {
        IBaseResponse<List<CategoryProduct>> GetCategories();

        Task<IBaseResponse<CategoryProduct>> Create(CategoryViewModel model);

        Task<IBaseResponse<bool>> DeleteCategory(int id);

        Task<IBaseResponse<CategoryProduct>> Edit(int id, CategoryViewModel model);

        Task<IBaseResponse<CategoryProduct>> GetCategory(int id);
    }
}
