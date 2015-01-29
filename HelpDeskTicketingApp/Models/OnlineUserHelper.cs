using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Models
{
    public class OnlineUserHelper
    {

        private static OnlineUserHelper _instance = null;

        private List<string> _userList;

        public static OnlineUserHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OnlineUserHelper();
                }
                return _instance;
            }
        }

        private OnlineUserHelper()
        {
            _userList = new List<string>();
        }


        public List<string> GetOnlineUsers()
        {
            return _userList;
        }

        public void AddUserAsOnline(string name)
        {
            if (!_userList.Contains(name))
            {
                  _userList.Add(name);
            var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();
            context.Clients.All.userConnected(name);
            }
          
        }
        public void RemoveUserAsOffline(string name)
        {
           _userList.Remove(name);
           var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();
           context.Clients.All.userDisconnected(name);
        
        }

    }
}