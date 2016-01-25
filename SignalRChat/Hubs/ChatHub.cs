using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    
    public class ChatHub : Hub
    {
        //public void Send(string name, string message)
        //{
        //    // Call the addNewMessageToPage method to update clients.
        //    Clients.All.addNewMessageToPage(name, message);

        //}

        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public void Send(string who, string message)
        {
            string name = Context.ConnectionId;

            foreach (var connectionId in _connections.GetConnections(who))
            {
                Clients.Client(connectionId).addNewMessageToPage(name, message);
            }
        }

        public override Task OnConnected()
        {
            string name = Context.QueryString["username"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

    }
}