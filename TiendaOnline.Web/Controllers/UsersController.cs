using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TiendaOnline.Web.Data;
using TiendaOnline.Web.Data.Entities;
using TiendaOnline.Web.Enums;
using TiendaOnline.Web.Interfaces;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        public UsersController(IUserHelper userHelper, 
            DataContext context, 
            ICombosHelper combosHelper,
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
            .Include(u => u.City)
            .ThenInclude(c => c.Department)
            .ThenInclude(s => s.Country)
            .ToListAsync());
            
        }
        public async Task<IActionResult> Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Id = Guid.Empty.ToString(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                Departments = await _combosHelper.GetComboDepartmentsAsync(1),
                Cities = await _combosHelper.GetComboCitiesAsync(1),
                UserType = UserType.Admin,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
               // if (model.ImageFile != null)
               // {
               //     imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
               // }
                User user = await _userHelper.AddUserAsync(model, imageId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public JsonResult? GetDepartments(int countryId)
        {
            Country? country = _context.Countries
            .Include(c => c.Departments)
            .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }
            return Json(country.Departments.OrderBy(d => d.Name));
            
        }
        public JsonResult? GetCities(int DepartmentId)
        {
            Department? Department = _context.Departments
            .Include(s => s.Cities)
            .FirstOrDefault(s => s.Id == DepartmentId);
            if (Department == null)
            {
                return null;
            }
            return Json(Department.Cities.OrderBy(c => c.Name));
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }
    }

}
