using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace chat_api.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage1(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveOne", user, message);
        }
    }
}