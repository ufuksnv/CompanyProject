using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CompanyProject.ViewModels;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddTodo(ToDo p)
        {
            TodoValidator todoValidator = new TodoValidator();
            ValidationResult result = todoValidator.Validate(p);
            if(result.IsValid)
            {
               p.TodoAddDate = DateTime.Now;
               p.Status = true;
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

        public IActionResult DeleteTodo(int id)
        {
            var values = _todoManager.TGetByID(id);
            
            if (values == null)
            {
                return NotFound();
            }
            else
            {
               // _todoManager.TDelete(values);
                values.Status = false;
                return View();
            }
        }

    }
}
