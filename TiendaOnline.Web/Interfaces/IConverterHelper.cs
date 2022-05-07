using System;
using System.Threading.Tasks;
using TiendaOnline.Web.Data.Entities;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Interfaces
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);

        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);

        ProductViewModel ToProductViewModel(Product product);

    }

}
