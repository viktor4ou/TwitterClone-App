using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Models.Models;

namespace SocialMedia.Models.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public ChatHub(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        public async Task SendMessage(string message)
        {
            var currentLoggedUser = signInManager.UserManager.GetUserAsync(Context.User).Result as ApplicationUser;
            if (currentLoggedUser is null)
            {
                throw new Exception("User not found");
            }
            string username = currentLoggedUser.FirstName + currentLoggedUser.LastName;
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
        //public override Task OnConnectedAsync()
        //{
        //    Clients.All.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined the chat");
        //}

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
