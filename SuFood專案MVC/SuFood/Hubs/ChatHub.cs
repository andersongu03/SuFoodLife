using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using Microsoft.AspNetCore.Http;

namespace SuFood.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SuFoodDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub(SuFoodDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SaveAndSendMessage(string message)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("GetAccountId");
            var user = await _context.Account
                .Where(a => a.AccountId == Convert.ToInt32(currentUserId))
                .Select(c => c.Account1)
                .FirstOrDefaultAsync();

            if (currentUserId != null && user != null)
            {
                var chatMessage = new Messages
                {
                    UserName = user,
                    Text = message,
                    When = DateTime.Now,
                    AccountId = Convert.ToInt32(currentUserId)
                };

                _context.Messages.Add(chatMessage);
                await _context.SaveChangesAsync();

                //歷史對話紀錄
                var chatHistory = await _context.Messages
                .OrderByDescending(m => m.When)
                .Take(10) // 假設只取最近的 10 則對話紀錄
                .ToListAsync();

                chatHistory.Reverse(); // 將對話紀錄反轉，以符合顯示的時間順序

                await Clients.All.SendAsync("ReceiveMessage", chatHistory);

                //await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }

        public async Task GetChatHistory()
        {
            var chatHistory = await _context.Messages
                .OrderByDescending(m => m.When)
                .Take(10) // 假設只取最近的 10 則對話紀錄
                .ToListAsync();

            chatHistory.Reverse(); // 將對話紀錄反轉，以符合顯示的時間順序

            await Clients.Caller.SendAsync("ReceiveMessage", chatHistory);
        }
    }
}
