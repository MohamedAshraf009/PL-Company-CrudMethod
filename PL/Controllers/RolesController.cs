using BLL.ViewModel;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager) 
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task<IActionResult> Index()
        {

            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.CreateAsync(identityRole);
                if (role.Succeeded)
                   return RedirectToAction("Index");
                
                foreach (var error in role.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(identityRole);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var Role = await roleManager.FindByIdAsync(id);

            if (Role is null)
            {
                return NotFound();
            }

            return View(Role);
        }
        public async Task<IActionResult> Update(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var Role = await roleManager.FindByIdAsync(id);

            if (Role is null)
            {
                return NotFound();
            }

            return View(Role);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, IdentityRole identityRole)
        {
            if (id != identityRole.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var row = await roleManager.FindByIdAsync(id);
                if (row is null)
                {
                    return NotFound();
                }
                row.Name = identityRole.Name;
                //row.NormalizedName = identityRole.NormalizedName;
                ///row.NormalizedName = identityRole.NormalizedName.ToUpper();

                var res = await roleManager.UpdateAsync(row);
                if (res.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in res.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(identityRole);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return NotFound();
            }


            var deleted = await roleManager.DeleteAsync(role);
            if (deleted.Succeeded)
                return RedirectToAction("Index");


            foreach (var error in deleted.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddOrRmoveRoleFromUser(string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role is null)
                return NotFound();
            var users = new List<UsersInRoles>();
            ViewBag.RoleId = RoleId;

            foreach(var user in userManager.Users)
            {
                var userInRole = new UsersInRoles()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                    userInRole.isSelected = true;
                else
                    userInRole.isSelected = false;
                
                users.Add(userInRole);

            }

            return View(users);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddOrRmoveRoleFromUser(string RoleId, List<UsersInRoles> usersInRoles)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var item in usersInRoles)
                {
                    var user = await userManager.FindByIdAsync(item.UserId);
                    if (user is not null)
                    {
                        if (item.isSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                            await userManager.AddToRoleAsync(user, role.Name);
                        else
                            await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                return RedirectToAction("Update", new {id = RoleId});
            }
            return View(usersInRoles);
  
        }
    }
}
    