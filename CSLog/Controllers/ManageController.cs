using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CSLog.Models;
using CSLog.Domain.Repositories;
using EzeTools.WebApps;
using EzeTools;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using CSLog.Domain.Entities;

namespace CSLog.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private Helper _hlp;
        private ApplicationDbContext cmd;
        ASPNetUserRepository _userRep;

        public ManageController()
        {
            _hlp = new Helper();
            cmd = new ApplicationDbContext();
            _userRep = new ASPNetUserRepository();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        //
        // GET: /Manage/Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = cmd.Users.ToList();
            ViewBag.Msg = TempData["Msg"];
            //ViewBag.User = new RegisterViewModel();
            return View(users);
        }


        public ActionResult Add()
        {
            return PartialView(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_hlp.IsValidEmail(model.Email))
                    {
                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName, MustChangePassword = true, EmailConfirmed = true };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            UserManager.AddToRole(user.Id, "User");
                            return Json(new { IsAuthenticated = true, IsSuccessful = true });
                        }
                        else
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = result.Errors.First() });
                        }
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid Email Address!" });
                    }
                }
                else
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "All fields are required!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }
        }



        public ActionResult Edit(string id)
        {
            var dt = _userRep.GetUserById(id);
            List<mRole> roles = new List<mRole>();
            foreach (var item in RoleManager.Roles.ToList())
            {
                var role = new mRole();
                role.Name = item.Name;
                if (UserManager.IsInRole(id, item.Name))
                    role.IsInRole = true;
                else
                    role.IsInRole = false;
                roles.Add(role);
            }
            ViewBag.Roles = roles;
            if (dt == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Bad Request!" });
            }
            return PartialView(dt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = _userRep.UpdateComplaint(model.Id, model.DisplayName,model.Status);
                    if (result)
                    {
                        if (model.Roles.Count() > 0)
                        {
                            var roles = await UserManager.GetRolesAsync(model.Id);
                            await UserManager.RemoveFromRolesAsync(model.Id, roles.ToArray());
                            foreach (var item in model.Roles)
                            {
                                UserManager.AddToRole(model.Id, item);
                            }
                        }
                        TempData["Msg"] = _hlp.getMsg(AlertType.success.ToString(), "Updated successfully!");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "An error occurred while updating user info!" });
                    }
                }
                else
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "All fields are required!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }
        }

        //public List<AspNetUser> GetUsersInSupportRole()
        //{
        //    var roleManager =
        //        new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //    var role = roleManager.FindByName("Support").Users.First();
        //    var usersInRole =
        //        cmd.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
        //    return usersInRole;
        //}


        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(string userId)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            ApplicationUser user = cmd.Users.Where(c => c.Id == userId).FirstOrDefault();
            model.UserId = userId;
            ViewBag.DisplayName = user.DisplayName;
            ViewBag.UserName = user.Email;
            return View(model);

        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "All fields are required");
                var user = cmd.Users.Where(c => c.Id == model.UserId).FirstOrDefault();
                ViewBag.DisplayName = user.DisplayName;
                ViewBag.UserName = user.Email;
                return View(model);
            }
             
            try
            {
                var user = cmd.Users.Where(c => c.Id == model.UserId).FirstOrDefault();
                ViewBag.DisplayName = user.DisplayName;
                ViewBag.UserName = user.Email;
                if (user != null)
                {
                    //if (user.UserName.ToLower() == "admin@xplugng.com")
                    //{
                    //    ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "The password to this account cannot be change!");
                    //    return View(model);
                    //}
                    var result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        _userRep.Update(user.Id, User.Identity.Name);
                        //if (!User.IsInRole("User"))
                        //{
                        //    UserManager.AddToRole(user.Id, "User");
                        //}
                        //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        //TempData["ChangedPassword"] = _hlp.getMsg(AlertType.success.ToString(), $"Password changed successfully. Please login");
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        AddErrors(result);
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "The user does not exist!");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = ErrorMessages.getMsg(ErrorCode.exception_error).msgText;
            }
            return View(model);

        }
        
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_hlp.IsValidEmail(model.Email))
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName, MustChangePassword = true, EmailConfirmed = true };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        ViewBag.User = new RegisterViewModel();
                        //UserManager.AddToRole(user.Id, "User");
                        ViewBag.Msg = _hlp.getMsg(AlertType.success.ToString(), "Added Successfully!");
                    }
                    else
                    {
                        ViewBag.User = model;
                        ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), result.Errors.First());
                    }
                }
                else
                {
                    ViewBag.User = model;
                    ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "Invalid Email Address!");
                }
            }
            else
            {
                ViewBag.User = model;
                ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "All fields are required!");
            }
            var users = cmd.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetPassword(string userId)
        {
            var user = cmd.Users.Where(c => c.Id == userId).FirstOrDefault();
            ViewBag.DisplayName = user.DisplayName;
            ViewBag.UserName = user.Email;
            string code = await UserManager.GeneratePasswordResetTokenAsync(userId);
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Code = code;
            model.Email = user.Email;
            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Email);
            ViewBag.DisplayName = user.DisplayName;
            ViewBag.UserName = user.Email;
            if (ModelState.IsValid)
            {
                try
                {

                    if (user != null)
                    {
                        var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                        if (result.Succeeded)
                        {

                            //if (User.Identity.Name.ToLower() == "admin@xplugng.com")
                            //    user.MustChangePassword = true;
                            //else
                            //    user.MustChangePassword = false;

                            ////cmd.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            //cmd.SaveChanges();
                            _userRep.Update(user.Id, User.Identity.Name);
                            //UserManager.RemoveFromRole(user.Id, "User");
                            ViewBag.Msg = _hlp.getMsg(AlertType.success.ToString(), $"The password has been reset! The new password is {model.Password}");
                        }
                        else
                        {

                            ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), result.Errors.First());
                        }

                    }
                    else
                        ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "This user does not exist!");
                }
                catch (Exception ex)
                {
                    ViewBag.Msg = ErrorMessages.getMsg(ErrorCode.exception_error).msgText;
                }
            }
            else
                ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "All fields are required!");

            return View(model);
        }


        #region Roles
        // GET: /Roles/
        public ActionResult Roles()
        {
            ViewBag.Msg = TempData["Msg"];
            return View(RoleManager.Roles);
        }

        public ActionResult AddRole()
        {
            return PartialView(new RoleViewModel());
        }


        [HttpPost]
        public async Task<ActionResult> AddRole(RoleViewModel roleViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (!RoleManager.RoleExists(roleViewModel.Name))
                    {
                        var role = new IdentityRole(roleViewModel.Name);
                        var roleresult = await RoleManager.CreateAsync(role);
                        if (roleresult.Succeeded)
                        {
                            TempData["Msg"] = _hlp.getMsg(AlertType.success.ToString(), "Added successfully!");
                            return RedirectToAction("Roles");
                        }
                        else
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = roleresult.Errors.First() });
                        }
                    }
                    else
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "The role already exist!" });
                    }
                    
                }
                else
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Please enter a valid role name!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = ex.Message });
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> Roles(RoleViewModel roleViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!RoleManager.RoleExists(roleViewModel.Name))
        //        {
        //            var role = new IdentityRole(roleViewModel.Name);
        //            var roleresult = await RoleManager.CreateAsync(role);
        //            if (!roleresult.Succeeded)
        //            {
        //                ViewBag.Msg = _hlp.getMsg(AlertType.success.ToString(), "Added successfully!");
        //                ModelState.AddModelError("", roleresult.Errors.First());
        //                return View(RoleManager.Roles);
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Msg = _hlp.getMsg(AlertType.danger.ToString(), "The role already exist!");
        //        }

        //    }
        //    return View(RoleManager.Roles);
        //}

        public async Task<ActionResult> RoleDetails(string id)
        {
            if (id == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Bad Request!" });
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Bad Request!" });
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Record not found!" });
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Bad Request!" });
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("Response", new ResponseViewModel() { Msg = "Record not found!" });
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return View("Response", new ResponseViewModel() { Msg = "Record not found!" });
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return View("Response", new ResponseViewModel() { Msg = "Record not found!" });
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}