using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using HelpDeskTicketingApp.Data.Models;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp
{
   
    [HubName("echo")]
    public class EchoHub : Hub
    {
        public void Say(string message)
        {
            Trace.WriteLine(message);
            //Clients.All.hello();
        }

        public void NewEntryOnTimeLine(TimeLineLog tl)
        {
            
           
            var allAdmins = Clients.All;
            allAdmins.newEntryTimeLine(tl);
          
           // Trace.WriteLine("I'm here");
        }
        public void UserConnected(string UserName)
        {
            var allAdmins = Clients.All;
            allAdmins.userConnected(UserName);
        }

        public void UserDisconnected(string UserName)
        {
            var allAdmins = Clients.All;
            allAdmins.userDisconnected(UserName);
        }

        public void NewTicketAdded()
        {
            var allAdmins = Clients.All;
            allAdmins.newTicketAdded();
        }

    }
}