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
    }
}
