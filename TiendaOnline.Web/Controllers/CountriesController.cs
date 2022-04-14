using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TiendaOnline.Web.Data;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries
                .Include(c => c.Departments)
                .ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(_c => _c.Departments)
                .ThenInclude(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }

            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(country);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            Country country = await _context.Countries
                .Include(c => c.Departments)
                .ThenInclude(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }



            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            Department model = new Department { IdCountry = country.Id };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                Country country = await _context.Countries
                .Include(c => c.Departments)
                .FirstOrDefaultAsync(c => c.Id == department.IdCountry);
                if (country == null)
                {
                    return NotFound();
                }
                try
                {
                    department.Id = 0;
                    country.Departments.Add(department);
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}", new { id = country.Id});
                    //return RedirectToAction($"{nameof(Details)}/{country.Id}");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {

                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> EditDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            department.IdCountry = country.Id;
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    var result = RedirectToAction(nameof(Details), new {id = department.IdCountry });
                    //return RedirectToAction($"{nameof(DetailsDepartment)}/{department.IdCountry}");
                    return result;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {

                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(department);
        }

        public async Task<IActionResult> DeleteDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department department = await _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            //return RedirectToAction($"{nameof(Details)}/{country.Id}");
            return RedirectToAction($"{nameof(Details)}",new { id = country.Id } );
        }

        public async Task<IActionResult> DetailsDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Department department = await _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            department.IdCountry = country.Id;
            return View(department);
        }


    }
}
