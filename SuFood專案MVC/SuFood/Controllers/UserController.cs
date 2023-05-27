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

namespace SuFood.Controllers
{

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
            var code = encryptService.Encrypt(jString);


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

            var str = encryptService.Decrypt(code);
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
            //return RedirectToAction("Enble", "User");
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
                var decryptedPassword = encryptService.Decrypt(user.Password);

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
        public IActionResult ForgetPassword4Email()
        {
            return PartialView();
        }
        public IActionResult ForgetPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult ForgetPassword4Email(ForgetPasswordViewModel model)
        {
            var user = _context.Account.FirstOrDefault(x => x.Account1 == model.Account1);


            if (user == null)
            {
                ViewBag.Error = "您還沒有建立帳號哦！！";
            }
            if (user.IsActive != true)
            {
                ViewBag.Error = "您還沒有啟用帳號哦！！";
            }

            //寄信
            var obj = new AesValidationDto(model.Account1, DateTime.Now.AddMinutes(30));
            var jString = JsonSerializer.Serialize(obj);
            var code = encryptService.Encrypt(jString);

            var mail = new MailMessage()
            {
                From = new MailAddress("SuFood2u@gmail.com"), //寄信的信箱
                Subject = "啟用帳號驗證", //主旨
                Body = (@$"請點<a href='https://localhost:50720/User/enableChangePassword?code={code}'>這裡</a>來修改你的新密碼"),
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
                throw;
            }
            return PartialView();
        }
        public async Task<IActionResult> EnableChangePassword(string code, ForgetPasswordViewModel model)
        {
            var str = encryptService.Decrypt(code);
            var obj = JsonSerializer.Deserialize<AesValidationDto>(str);
            if (DateTime.Now > obj.ExpiredDate)
            {
                return BadRequest("過期");
            }
            //return Ok($@"code:{code}  str:{str}");
            return RedirectToAction("ForgetPassword", "User");
        }
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var user = _context.Account.FirstOrDefault(x => x.Account1 == model.Account1);
            if (user.Password != model.ConfirmPwd)
            {
                ViewBag.Error = "確認密碼不一致";                
            }

            var encryptedPassword = encryptService.Encrypt(model.Password);

            _context.Account.Update(new Account()
            {                
                Password = encryptedPassword,
            });

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult ValidGoogleLogin()
        //{
        //    string? formCredential = Request.Form["credential"]; //回傳憑證
        //    string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
        //    string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

        //    // 驗證 Google Token
        //    GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
        //    if (payload == null)
        //    {
        //        // 驗證失敗
        //        ViewData["Msg"] = "驗證 Google 授權失敗";
        //    }
        //    else
        //    {
        //        //驗證成功，取使用者資訊內容
        //        ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
        //        ViewData["Msg"] += "Email:" + payload.Email + "<br>";
        //        ViewData["Msg"] += "Name:" + payload.Name + "<br>";
        //        ViewData["Msg"] += "Picture:" + payload.Picture;
        //    }

        //    return View();
        //}

        ///// <summary>
        ///// 驗證 Google Token
        ///// </summary>
        ///// <param name="formCredential"></param>
        ///// <param name="formToken"></param>
        ///// <param name="cookiesToken"></param>
        ///// <returns></returns>
        //public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        //{
        //    // 檢查空值
        //    if (formCredential == null || formToken == null && cookiesToken == null)
        //    {
        //        return null;
        //    }

        //    GoogleJsonWebSignature.Payload? payload;
        //    try
        //    {
        //        // 驗證 token
        //        if (formToken != cookiesToken)
        //        {
        //            return null;
        //        }

        //        // 驗證憑證
        //        IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
        //        string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
        //        var settings = new GoogleJsonWebSignature.ValidationSettings()
        //        {
        //            Audience = new List<string>() { GoogleApiClientId }
        //        };
        //        payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
        //        if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
        //        {
        //            return null;
        //        }
        //        if (payload.ExpirationTimeSeconds == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            DateTime now = DateTime.Now.ToUniversalTime();
        //            DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
        //            if (now > expiration)
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return payload;
        //}

    }
}
