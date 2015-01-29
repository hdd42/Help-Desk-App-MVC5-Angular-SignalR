using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTicketingApp.Adapters.Interfaces
{
   public interface ITimeLineAdapter
    {
       List<TimeLine> GetAll();
       void AddNewEntry(TimeLineLog t);


       AdminDashboardVidewModel InitDashBoard();
    }
}
