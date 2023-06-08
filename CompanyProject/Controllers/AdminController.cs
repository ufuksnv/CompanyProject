using BusinessLayer.Concrete;
using CompanyProject.ViewModels;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CompanyProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        AdviceManager _adviceManager = new AdviceManager(new EfAdviceDal());
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdviceList()
        {
            var request = _adviceManager.TGetList();            
            return View(request);
        }

        public IActionResult DeleteAdvice(int id)
        {
            var values = _adviceManager.TGetByID(id);

            if (values == null)
            {
                return NotFound();
            }
            else
            {
                _adviceManager.TDelete(values);
                return RedirectToAction("AdviceList", "Admin");
            }
        }

        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            AppRole appRole = new AppRole()
            {
                Name = request.Name,
            };

            var identityResult = await _roleManager.CreateAsync(appRole);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }

            }

            return RedirectToAction(nameof(AdminController.Index));
        }

    }
}
