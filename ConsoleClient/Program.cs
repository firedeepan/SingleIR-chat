using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static  void Main(string[] args)
        {
            CreateConnection();
            Console.ReadKey();
        }

        public static async void CreateConnection()
        {
            var hubConnection = new HubConnection("http://localhost:62407/");
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("chatHub");
            stockTickerHubProxy.On<string,string>("addNewMessageToPage", (sender,message) => Console.WriteLine("Stock update for {0} new price {1}", sender, message));
            await hubConnection.Start();

            await stockTickerHubProxy.Invoke("Send","Deepan","Hello");
        }
    }
}
