using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task EnterMessage(string user)
        {
            await Clients.All.SendAsync("ReceiveEnterMessage", user);
        }

       
        // 連線
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        // 斷線
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
