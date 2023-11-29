using AutoMapper;
using BLL.ViewModel;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> _userManager) 
        {
            userManager = _userManager;
        }
        public IActionResult Index(string SearchValue= "")
        {
            //if (string.IsNullOrEmpty(SearchValue))
            //{
           var res = userManager.Users.ToList();
           return View(res);
            //}
            //else
            //{
            //    var res = await userManager.FindByNameAsync(SearchValue);
            //    return View(new List<ApplicationUser>{ res});
            //}
        }

        public async Task<IActionResult> Details(string id)
        {
            if(id is null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id,ApplicationUser Newdata)
        {
            if (id is null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var row = await userManager.FindByIdAsync(id);
                if (row is null)
                {
                    return NotFound();
                }
                row.PhoneNumber = Newdata.PhoneNumber;
                row.UserName = Newdata.UserName;
                row.NormalizedUserName = Newdata.UserName.ToUpper();
                
                var res= await userManager.UpdateAsync(row);
                if(res.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in res.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(Newdata);
        }
        public async Task<IActionResult> delete(string id)
        {
            if (id is null)
                return BadRequest();

            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var deleted = await userManager.DeleteAsync(user);
            if (deleted.Succeeded)
                return RedirectToAction("Index");


            foreach (var error in deleted.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToAction("Index");
        }
    }


}
