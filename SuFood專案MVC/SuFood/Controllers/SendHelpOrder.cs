using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SuFood.Data;
using SuFood.Models;
using SuFood.Services;

namespace SuFood.Controllers
{
    public class SendHelpOrder
    {
        public class HangfireController : Controller
        {   
            private readonly IBackgroundJobClient _backgroundJobClient;
            private readonly SuFoodDBContext _context;
            private readonly IEmailService _emailService;

            public HangfireController(IBackgroundJobClient backgroundJobClient, SuFoodDBContext context, IEmailService emailService)
            {
                _backgroundJobClient = backgroundJobClient;
                _context = context;
                _emailService = emailService;
            }

            public IActionResult ScheduleJob()
            {
                RecurringJob.AddOrUpdate(() => SendEmailJob(), Cron.Daily(0, 0));
                return Ok("Job scheduled successfully.");
            }

            public void SendEmailJob()
            {
                var orders = _context.RecyleSubscribeOrders.Where(x => x.ShipDate.Date == DateTime.Today).ToList();

                foreach (var order in orders)
                {
                    var Order = _context.Orders.Where(x=>x.OrdersId == order.OrdersId).FirstOrDefault();

                    var sendEmail = Order.Email;
                    var subject = "感謝訂購";
                    var body = $"{Order.Name}您好，您的訂單編號訂單編號{Order.OrdersId}，於今日{DateTime.Today}成功寄送，請期待我們隨機幫您搭配的商品，SuFoodLife蔬服人生感謝您~";

                    _emailService.SendEmail(sendEmail, subject, body);
                }
            }
        }
    }
}
