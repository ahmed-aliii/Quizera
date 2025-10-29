using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizera.Domain;
using System.Security.Claims;

namespace Quizera.WebUI.Controllers
{
    public class AccountController : Controller
    {

        #region Dependency Injection

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController
            (
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #endregion


        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignupVM SignupVM)
        {
            #region Not Valid
            if (!ModelState.IsValid)
            {
                return View("SignUp", SignupVM);
            }
            #endregion

            #region Valid

            //1)Map VM To ApplicationUser(Without Password because it passed after hashing to user)
            var user = new ApplicationUser()
            {
                UserName = SignupVM.UserName,
                Email = SignupVM.Email,
            };

            //2)Save To DB
            var createUserResult = await _userManager.CreateAsync(user, SignupVM.Password);

            //3) Fail : Fail To Add User To DB
            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                //return to View
                return View("SignUp", SignupVM);
            }

            //4)Success : To Success To Add User To DB 
            #region 3) Create Cookie And Add Role
            //Get User From DB
            var UserFromDB = await _userManager.FindByEmailAsync(user.Email);

            //Add Role To User
            var addRoleResult = await _userManager.AddToRoleAsync(UserFromDB, SignupVM.Role);

            //SignInAsync : Create Cookie For this User
            await _signInManager.SignInAsync(user, isPersistent: false);

            //Redirect To Home View
            return RedirectToAction("Index", "Home");
            #endregion

            #endregion

        }
        #endregion


        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in → go to Home
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM LoginVM)
        {
            #region NOT Valid
            if (!ModelState.IsValid)
            {
                return View("Login", LoginVM);
            }
            #endregion

            #region Valid
            //Get User From DB
            var user = await _userManager.FindByEmailAsync(LoginVM.Email);

            //Fail: usr is null
            if (user == null)
            {
                ModelState.AddModelError("", "Email or Password Are InCorrect");

                return View("Login", LoginVM);
            }

            //Success: User Not Null
            //Check Password
            var IsValid = await _userManager.CheckPasswordAsync(user, LoginVM.Password);
            if (!IsValid) //password incorrect
            {
                ModelState.AddModelError("", "Password Is InCorrect");
                return View("Login", LoginVM);
            }

            //Email And Password Are Correct
            #region Add Claims
            //Configure User Claims
            //Default Claims NameIdentitfier , Name , Role (ID , UserName , Role)

            //Extra Cliams
            var email = new Claim("UserEmail", user.Email);
            //Claims List
            List<Claim> litsOfExtraClaims = new List<Claim>()
                {
                    email,
                };
            #endregion

            //Create Cookie With Claims
            await _signInManager.SignInWithClaimsAsync(user, isPersistent: LoginVM.RememberMe, litsOfExtraClaims);

            return RedirectToAction("Index", "Home");
            #endregion
        }
        #endregion


        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            //SignOutAsync → “remove the cookie so the system forgets the user.”
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        #endregion


        #region Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

    }
}
