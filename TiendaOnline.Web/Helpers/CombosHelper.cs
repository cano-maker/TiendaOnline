using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaOnline.Web.Data;
using TiendaOnline.Web.Interfaces;

namespace TiendaOnline.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;
        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync()
        {
            List<SelectListItem> list = await _context.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();
            list.Insert(0, new SelectListItem

            {
                Text = "[Seleccione una categoría...]",
                Value = "0"
            });
            return list;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int DepartmentId)
        {
            List<SelectListItem> list = await _context.Cities
            .Where(x => x.IdDepartment == DepartmentId)
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una ciudad...]",
                Value = "0"
            });
            return list;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un país...]",
                Value = "0"
            });
            return list;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboDepartmentsAsync(int countryId)
        {
            List<SelectListItem> list = await _context.Departments
            .Where(x => x.IdCountry == countryId)
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = $"{x.Id}"
            })
            .OrderBy(x => x.Text)
            .ToListAsync();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un departamento/estado...]",
                Value = "0"
            });
            return list;
        }
    }
}
