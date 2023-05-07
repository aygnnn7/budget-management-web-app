using FamilyBudget.ActionFilters;
using FamilyBudget.Entities;
using FamilyBudget.ExtensionMethods;
using FamilyBudget.Models.User;
using FamilyBudget.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Controllers
{
    public class UserController : Controller
    {
        [OnlyNotLogged]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet,OnlyNotLogged]
        public IActionResult Save(bool IsRegister)
        {
            return View(new SaveUserVM()
            {
                IsRegister = IsRegister
            });
        }

        [HttpPost,OnlyNotLogged]
        public IActionResult Save(SaveUserVM model)
        {
            Db dbContext = new Db();
            User? user;
            
            if (model.IsRegister)
            {
                if (!ModelState.IsValid) return View(model);
                bool emailAlreadyExists = dbContext.users.Where(u => u.Email == model.Email).Any();
                if (emailAlreadyExists)
                {
                        ModelState.AddModelError("ErrorSummary", $"User with email {model.Email} already exists!");
                        return View(model);
                }

                user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    RegisteredTime = DateTime.Now,
                    Sex = (model.Sex == Sex.Man)
                };
                dbContext.Add(user);
                dbContext.SaveChanges();
            }

            else
            {
                user = dbContext.users?.Where(u => u.Email.Equals(model.Email)
                                                         && u.Password.Equals(model.Password)).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("ErrorSummary", $"Wrong email or password!");
                    return View(model);
                };
            }
            user.Password.Remove(0);
            HttpContext.Session.SetObject("loggedUser", user);
            return RedirectToAction("Index", "Home");
        }

        [AuthFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");
            return RedirectToAction("Save", "User", false);
        }

        [HttpGet, AuthFilter]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet, AuthFilter]
        public IActionResult? Edit() {
            User? loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            return View(new EditVM()
            {
                Id = loggedUser.Id,
                Email = loggedUser.Email,
                Name = loggedUser.Name,
                Sex = loggedUser.Sex ? Sex.Man : Sex.Woman
            });
        }

        [HttpPost, AuthFilter]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid) return View(model);
            User? loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            Db dbContext = new Db();
            bool emailAlreadyExists = model.Email != loggedUser.Email && dbContext.users.Where(u => u.Email == model.Email).Any();
            if (emailAlreadyExists)
            {
                ModelState.AddModelError("ErrorSummary", $"User with email {model.Email} already exists!");
                return View(model);
            }

            User? user = new User()
            {
                Id = model.Id,
                Email = model.Email,
                Name = model.Name,
                Sex = model.Sex == Sex.Man,
                Password = loggedUser.Password,
                RegisteredTime =  loggedUser.RegisteredTime
            };
            
            dbContext.Entry(user).State = EntityState.Detached;
            dbContext.users.Update(user);
            dbContext.SaveChanges();
            
            HttpContext.Session.Remove("loggedUser");
            HttpContext.Session.SetObject<User>("loggedUser", user);
            return RedirectToAction("Profile", "User");
        }

        [AuthFilter]
        public IActionResult Delete()
        {
            Db context = new Db();
            User? user = HttpContext.Session.GetObject<User>("loggedUser");
            context.users.Remove(user);
            context.SaveChanges();
            HttpContext.Session.Remove("loggedUser");
            
            return RedirectToAction("Index", "Home");
        }
    }
}

