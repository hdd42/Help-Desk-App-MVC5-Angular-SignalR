using HelpDeskTicketingApp.Adapters.DataAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Adapters
{
    public static class TicketAdapterFactory
    {
        public static ITicketAdapter GetTicketAdapter()
        {
            return new TicketDataAdapter();
        }
    }
}