using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProject.Controllers
{
    public class AdminController : Controller
    {
        AdviceManager _adviceManager = new AdviceManager(new EfAdviceDal());

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
                return View();
            }
        }
    }
}
