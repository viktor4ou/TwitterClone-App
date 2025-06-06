﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.Models.Models;

namespace SocialMedia.Models.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private Dictionary<string, string> Connections;

        public ChatHub(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            Connections = new Dictionary<string, string>();
        }
        public async Task SendMessage(string message)
        {
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(Context.User);
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
        public async Task SendPrivateMessage(string userToMessageId, string message)
        {
            //The button is going to route the userId that has to be messaged
            //Everytime that someone else if selected for chat , check if there is connection and close if exists
            //When new connection is open we want to load the messages for the userToMessageId and the currentUserId
            var currentLoggedUser = await signInManager.UserManager.GetUserAsync(Context.User);
            string username = currentLoggedUser.FirstName + currentLoggedUser.LastName;
            message = "seks";
            //how can i have new connection when i click on a button
            await Clients.Client("1fc7a851-f365-4ec6-9e5a-0a4d6ff36703").SendAsync("ReceiveMessage", username, message);
            //
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
