﻿using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProject.Controllers
{
    public class MemberController : Controller
    {
        AdviceManager _adviceManager = new AdviceManager(new EfAdviceDal());
        ToDoManager _todoManager = new ToDoManager(new EfToDoDal());


         

        public IActionResult Index()
        {
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

    }
}
