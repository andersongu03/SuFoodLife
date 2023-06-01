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
using Google.Apis.Auth;
using System.Security.Cryptography.Xml;


namespace SuFood.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly SuFoodDBContext _context;
        private readonly EncryptService encryptService;
        public UserController(SuFoodDBContext context, EncryptService encryptService)
        {
            this._context = context;
            this.encryptService = encryptService;
        }
        [HttpGet]        
        public IActionResult Login()
        {
            return PartialView();
        }        
        public IActionResult Register()
        {
            return PartialView();
        }
        public IActionResult EnterComfirmPassword()
        {
            return PartialView();
        }
        public IActionResult Enble()
        {
            return PartialView();
        }
        public IActionResult ForgetPassword4Email()
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

            var encryptedPassword = encryptService.Encrypt(model.Password);

            _context.Account.Add(new Account()
            {
                Account1 = model.Account1,
                Password = encryptedPassword,
                Role = "Customer",
                IsActive = false
            });

            _context.SaveChanges();
            //寄信
            var obj = new AesValidationDto(model.Account1, DateTime.Now.AddDays(1));
            var jString = JsonSerializer.Serialize(obj);
            var code = Convert.ToBase64String(Encoding.UTF8.GetBytes(jString));


            var mail = new MailMessage()
            {
                From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
                Subject = "啟用帳號驗證", //主旨
                Body = (@$"請點<a href='https://localhost:50720/User/enable?code={code}'>這裡</a>來啟用你的帳號"),
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
                    ViewBag.Succuss = "已成功註冊，請至信箱收取驗證信";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView();
        }
        public async Task<IActionResult> Enable(string code)
        {
            var str = Convert.FromBase64String(code);
            var obj = JsonSerializer.Deserialize<AesValidationDto>(Encoding.UTF8.GetString(str));
            if (DateTime.Now > obj.ExpiredDate)
            {
                return BadRequest("過期");
            }
            var user = _context.Account.FirstOrDefault(x => x.Account1 == obj.Account);
            if (user != null)
            {
                user.IsActive = true;
                user.CreateDatetime = DateTime.Now;
                _context.SaveChanges();
            }
            //return Ok($@"code:{code}  str:{str}");
            return RedirectToAction("Enble", "User");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, bool RememberAcc)
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
                var decryptedPassword = encryptService.Decrypt(user.Password);

                if (model.Password != decryptedPassword)
                {
                    ViewBag.Error = "帳號密碼錯誤！";
                    return PartialView("Login");
                }
            }
            if (RememberAcc)
            {                
                Response.Cookies.Append("Account1", model.Account1, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30) // 存在Cookie30天
                });
            }
            else
            {
                Response.Cookies.Delete("Account1");
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal);

            HttpContext.Session.SetString("GetAccountId", Convert.ToString(user.AccountId));

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ForgetPassword4Email(ForgetPasswordViewModel model)
        {
            var user = _context.Account.FirstOrDefault(x => x.Account1 == model.Account1);

            if (user == null)
            {
                ViewBag.Error = "您還沒有建立帳號哦！！";
                return View();
            }
            if (user.IsActive != true)
            {
                ViewBag.Error = "您還沒有啟用帳號哦！！";
                return View();  
            }

            //寄信
            var obj = new AesValidationDto(model.Account1, DateTime.Now.AddMinutes(30));
            var jString = JsonSerializer.Serialize(obj);
            var code = Convert.ToBase64String(Encoding.UTF8.GetBytes(jString));

            var mail = new MailMessage()
            {
                From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
                Subject = "啟用帳號驗證", //主旨
                Body = (@$"請點<a href='https://localhost:50720/User/enableChangePassword?code={code}'>這裡</a>來修改你的密碼"),
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
                    ViewBag.Succuss = "驗證信已寄出，限時30分鐘！！";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView();
        }
        public async Task<IActionResult> EnableChangePassword(string code, ForgetPasswordViewModel model)
        {
            var str = Convert.FromBase64String(code);
            var obj = JsonSerializer.Deserialize<AesValidationDto>(str); 
            //序列化json格式            
            string json = JsonSerializer.Serialize(obj);
            //反序列化json格式
            HttpContext.Session.SetString("obj", json);

            if (DateTime.Now > obj.ExpiredDate)
            {
                return BadRequest("過期");
            }
            
            return RedirectToAction("EnterComfirmPassword", "User");
            //return Ok($@"code:{code}  str:{str}");
        }
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            string obj = HttpContext.Session.GetString("obj");

            if (string.IsNullOrEmpty(obj))
            {               
                return Ok("驗證信沒過");               
            }
            AesValidationDto objOK = JsonSerializer.Deserialize<AesValidationDto>(obj);

            var user = _context.Account.FirstOrDefault(x => x.Account1 == objOK.Account);

            if (model.Password != model.ConfirmPwd)
            {
                ViewBag.Error = "確認密碼不一致";
            }
            else if (user != null)
            {
                var encryptedPassword = encryptService.Encrypt(model.Password);
                user.Password = encryptedPassword;
                _context.Account.Update(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Enble", "User");
        }


    }
}
