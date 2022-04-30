using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaOnline.Web.Interfaces
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync();
        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();
        Task<IEnumerable<SelectListItem>> GetComboDepartmentsAsync(int countryId);
        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int DepartmentId);
    }

}
