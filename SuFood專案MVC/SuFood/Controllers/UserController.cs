using SuFood.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuFood.Models;
using SuFood.ViewModel;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace SuFood.Controllers
{

    public class UserController : Controller
    {
        private readonly SuFoodDBContext _context;
        private readonly EncryptService encrypt;
        public UserController(SuFoodDBContext context, EncryptService encrypt)
        {
            this._context = context;
            this.encrypt = encrypt;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = _context.Account.FirstOrDefault(x => x.Account1 == model.Account1);
            if (user != null)
            {
                ViewBag.Error = "帳號已經存在";
                return PartialView("Register");
            }
            if (model.ConfirmPwd != model.Password)
            {
                ViewBag.Error = "確認密碼不一致";
                return PartialView("Register");
            }

            var encryptedPassword = encrypt.AesEncryptToBase64(model.Password);

            _context.Account.Add(new Account()
            {
                Account1 = model.Account1,
                Password = encryptedPassword,
                Role = "Customer",
                IsActive = false
            });

            _context.SaveChanges();
            //寄信
            var obj = new AesValidationDto(model.Account1, DateTime.Now.AddDays(3));
            var jString = JsonSerializer.Serialize(obj);
            var code = encrypt.AesEncryptToBase64(jString);


            var mail = new MailMessage()
            {
                From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
                Subject = "啟用帳號驗證", //主旨
                Body = (@$"請點<a href='https://localhost:7086/User/enable?code={code}'>這裡</a>來啟用你的帳號"),
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };
            mail.To.Add(new MailAddress(model.Account1)); //寄給哪位
            try
            {
                using (var sm = new SmtpClient("smtp.gmail.com", 587)) //465 ssl
                {
                    sm.EnableSsl = true;
                    sm.Credentials = new NetworkCredential("SuFood2u@gmail.com", "okjzbnxgsmkmfwlq");
                    sm.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Enable(string code)
        {
            var str = encrypt.AesDecryptToString(code);
            var obj = JsonSerializer.Deserialize<AesValidationDto>(str);
            if (DateTime.Now > obj.ExpiredDate)
            {
                return BadRequest("過期");
            }
            var user = _context.Account.FirstOrDefault(x => x.Account1 == obj.Account);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }

            return Ok($@"code:{code}  str:{str}");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Account.FirstOrDefault(Acc => Acc.Account1 == model.Account1 && Acc.IsActive == true);

            if (user == null)
            {
                ViewBag.Error = "帳號不存在或未啟用！";
                return PartialView("Login");
            }

            if (user.Role == "Employee")
            {
                // 在 Role 是 "Employee" 的情況下，進行一般的密碼驗證
                if (model.Password != user.Password)
                {
                    ViewBag.Error = "管理者的密碼錯誤！";
                    return PartialView("Login");
                }
            }
            else if (user.Role == "Customer")
            {
                // 在 Role 是 "Customer" 的情況下，使用解密的方式驗證密碼
                var decryptedPassword = encrypt.AesDecryptToString(user.Password);

                if (model.Password != decryptedPassword)
                {
                    ViewBag.Error = "帳號密碼錯誤！";
                    return PartialView("Login");
                }
            }

            //建立憑證 (訂定身分證上的欄位) 
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Account1),
                new Claim(ClaimTypes.Role, user.Role)
            };

            //制定身分證驗證上面要根據那些欄位驗證，並使用Cookie驗證
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //宣告一個帳號有幾個憑證            
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //登入並給一張憑證
            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
