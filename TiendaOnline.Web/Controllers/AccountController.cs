using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TiendaOnline.Web.Common;
using TiendaOnline.Web.Data;
using TiendaOnline.Web.Data.Entities;
using TiendaOnline.Web.Enums;
using TiendaOnline.Web.Interfaces;
using TiendaOnline.Web.Models;

namespace TiendaOnline.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;
        public AccountController(IUserHelper userHelper,
            DataContext context,
            ICombosHelper combosHelper,
            IBlobHelper blobHelper,
            IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;

        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitar el usuario.");
                }

            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        //public async Task<IActionResult> Register()
        //{
        //    AddUserViewModel model = new AddUserViewModel
        //    {
        //        Id = Guid.Empty.ToString(),
        //        Countries = await _combosHelper.GetComboCountriesAsync(),
        //        Departments = await _combosHelper.GetComboDepartmentsAsync(0),
        //        Cities = await _combosHelper.GetComboCitiesAsync(0),
        //        //UserType = UserType.User,
        //    };
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(AddUserViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        Guid imageId = Guid.Empty;
        //        //if (model.ImageFile != null)
        //        //{
        //        //    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
        //        //}
        //        User user = await _userHelper.AddUserAsync(model, imageId);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
        //            return View(model);
        //        }
        //        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        //        string tokenLink = Url.Action("ConfirmEmail", "Account", new
        //        {
        //            userid = user.Id,
        //            token = myToken
        //        }, protocol: HttpContext.Request.Scheme);
        //        Response response = _mailHelper.SendMail(
        //        $"{model.FirstName} {model.LastName}",
        //        model.Username,
        //        "Shopping - Confirmación de Email",
        //        $"<h1>Shopping - Confirmación de Email</h1>" +
        //        $"Para habilitar el usuario por favor hacer clicn en el siguiente link:, " +
        //        $"<p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
        //        if (response.IsSuccess)
        //        {
        //            ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
        //            return View(model);
        //        }
        //        ModelState.AddModelError(string.Empty, response.Message);
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult> ChangeUser()
        //{
        //    User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    EditUserViewModel model = new EditUserViewModel
        //    {
        //        Address = user.Address,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        PhoneNumber = user.PhoneNumber,
        //        ImageId = user.ImageId,
        //        Cities = await _combosHelper.GetComboCitiesAsync(user.City.Department.Id),
        //        CityId = user.City.Id,
        //        Countries = await _combosHelper.GetComboCountriesAsync(),
        //        CountryId = user.City.Department.Country.Id,
        //        DepartmentId = user.City.Department.Id,
        //        Departments = await _combosHelper.GetComboDepartmentsAsync(user.City.Department.Country.Id),
        //        Id = user.Id,
        //        Document = user.Document
        //    };
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Guid imageId = model.ImageId;
        //        if (model.ImageFile != null)
        //        {
        //            imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
        //        }
        //        User user = await _userHelper.GetUserAsync(User.Identity.Name);
        //        user.FirstName = model.FirstName;
        //        user.LastName = model.LastName;
        //        user.Address = model.Address;
        //        user.PhoneNumber = model.PhoneNumber;
        //        user.ImageId = imageId;
        //        user.City = await _context.Cities.FindAsync(model.CityId);
        //        user.Document = model.Document;
        //        await _userHelper.UpdateUserAsync(user);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(model);
        //}

        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userHelper.GetUserAsync(User.Identity.Name);
        //        if (user != null)
        //        {
        //            var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("ChangeUser");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "User no found.");
        //        }
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        //    {
        //        return NotFound();
        //    }
        //    User user = await _userHelper.GetUserAsync(new Guid(userId));
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
        //    if (!result.Succeeded)
        //    {
        //        return NotFound();
        //    }
        //    return View();
        //}

        //public IActionResult RecoverPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)

        //    {
        //        User user = await _userHelper.GetUserAsync(model.Email);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "El email no corresponde a ningún usuario registrado.");
        //            return View(model);
        //        }
        //        string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
        //        string link = Url.Action(
        //        "ResetPassword",
        //        "Account",
        //        new { token = myToken }, protocol: HttpContext.Request.Scheme);
        //        _mailHelper.SendMail(
        //        $"{user.FullName}",
        //        model.Email,
        //        "Shopping - Recuperación de Contraseña",
        //        $"<h1>Shopping - Recuperación de Contraseña</h1>" +
        //        $"Para recuperar la contraseña haga click en el siguiente enlace:" +
        //        $"<p><a href = \"{link}\">Reset Password</a></p>");
        //        ViewBag.Message = "Las instrucciones para recuperar la contraseña han sido enviadas a su correo.";
        //        return View();
        //    }
        //    return View(model);
        //}
        //public IActionResult ResetPassword(string token)
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    User user = await _userHelper.GetUserAsync(model.UserName);
        //    if (user != null)
        //    {
        //        IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
        //        if (result.Succeeded)
        //        {
        //            ViewBag.Message = "Contraseña cambiada con éxito.";
        //            return View();
        //        }
        //        ViewBag.Message = "Error cambiando la contraseña.";
        //        return View(model);
        //    }
        //    ViewBag.Message = "Usuario no encontrado.";
        //    return View(model);
        //}

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Countries = _combosHelper.GetComboCountries(),
                Departments = _combosHelper.GetComboDepartments(0),
                Cities = _combosHelper.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Countries = _combosHelper.GetComboCountries();
                    model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
                    model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail($"{model.FirstName} {model.LastName}"
                    , model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            model.Countries = _combosHelper.GetComboCountries();
            model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
            return View(model);
        }


        public JsonResult GetDepartments(int countryId)
        {
            Country country = _context.Countries
                .Include(c => c.Departments)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.Departments.OrderBy(d => d.Name));
        }

        public JsonResult GetCities(int departmentId)
        {
            Department department = _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefault(d => d.Id == departmentId);
            if (department == null)
            {
                return null;
            }

            return Json(department.Cities.OrderBy(c => c.Name));
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities.FirstOrDefault(c => c.Id == user.City.Id) != null);
            if (department == null)
            {
                department = await _context.Departments.FirstOrDefaultAsync();
            }

            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.Id == department.Id) != null);
            if (country == null)
            {
                country = await _context.Countries.FirstOrDefaultAsync();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Cities = _combosHelper.GetComboCities(department.Id),
                CityId = user.City.Id,
                Countries = _combosHelper.GetComboCountries(),
                CountryId = country.Id,
                DepartmentId = department.Id,
                Departments = _combosHelper.GetComboDepartments(country.Id),
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.City = await _context.Cities.FindAsync(model.CityId);
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
            model.Countries = _combosHelper.GetComboCountries();
            model.Departments = _combosHelper.GetComboDepartments(model.CityId);
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail($"{user.FullName}", model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }




    }
}
