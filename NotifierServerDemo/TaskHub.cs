using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NotifierServerDemo
{
    [Authorize]
    public class TaskHub : Hub
    {
        public async void Test()
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "Test");
        }
    }
}
