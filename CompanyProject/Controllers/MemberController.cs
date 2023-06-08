using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CompanyProject.ViewModels;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace CompanyProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        AdviceManager _adviceManager = new AdviceManager(new EfAdviceDal());
        ToDoManager _todoManager = new ToDoManager(new EfToDoDal());

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        //anasayfaya kişisel bilgi eklenecek(bitti), todolar silinebilecek(bitti),
        //admin tarafında öneriler silinecek(bitti), facebook ile giriş olabilir.
        //todo list yapılacak(bitti)(renk ayarlaması yapılabilir)
        // kişisel bilgileri güncelleme eklenebilir.
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
            };

            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {           

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,              
            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDay = request.BirthDay;
                       


            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                foreach (IdentityError item in updateToUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Bilgileriniz güncellenmiştir";


            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,              
            };

            return View(userEditViewModel);
        }

        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                foreach (IdentityError item in resultChangePassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifreniz değiştirilmiştir";

            return View();
        }

        [HttpGet]
        public IActionResult AddAdvice()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAdvice(Advice p)
        {
            AdviceValidator adviceValidator = new AdviceValidator();
            ValidationResult results = adviceValidator.Validate(p);
            if (results.IsValid)
            {
                p.AdviceDate = DateTime.Now;
                p.Status = true;
                _adviceManager.TAdd(p);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddTodo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(ToDo p)
        {
            TodoValidator todoValidator = new TodoValidator();
            ValidationResult result = todoValidator.Validate(p);
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            if (result.IsValid)
            {
               p.TodoAddDate = DateTime.Now;
               p.Status = true;
               p.AppUserId = currentUser.Id;
                _todoManager.TAdd(p);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        public IActionResult TodoList()
        {
            var request = _todoManager.TGetList();
            return View(request);
        }

        public async Task< IActionResult> TodoListByMember()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var memberId = currentUser.Id;
            var values = _todoManager.GetTodoListByMember(memberId);
            return View(values);
        }

        public IActionResult DeleteTodo(int id)
        {
            var values = _todoManager.TGetByID(id);
            
            if (values == null)
            {
                return NotFound();
            }
            else
            {
                _todoManager.TDelete(values);
                return RedirectToAction("Index", "Member");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Home");
        }
    }
}
