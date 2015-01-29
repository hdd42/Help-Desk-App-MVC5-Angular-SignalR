using HelpDeskTicketingApp.Adapters.DataAdapters;
using HelpDeskTicketingApp.Adapters.Interfaces;
using HelpDeskTicketingApp.Data.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Models
{
    public class TimeLineViewModel
    {
        List<TimeLine> timelines { get; set; }
    }

    public class TimeLine
    {
        public int EntryId { get; set; }

        public string UserName { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime? TimeHappened { get; set; }
        public string EntryType { get; set; }
    }

    public static class TimeLineHelper
    {

        private static ITimeLineAdapter _adapter = new TimeLineDataAdapter();


        public  enum Operations
        {
            Edit,
            New,
            Delete
        }
        public enum OperationsFor
        {
            User,
            Ticket,
            Assigment
        }
    
      
        public static void UserOperations(string UserFullName, string ByUser ,Operations operation, string message="")
        {
           
            TimeLineLog tl = new TimeLineLog()
            {
               
                Notes = "Notes : " + message == "" ? "No message" : message,         
                TimeHappened = DateTime.Now,
                UserName = ByUser
            };

            if (operation == Operations.New)
            {
                
                    tl.Description = "New User has been created! Name :  " +UserFullName ;
                    tl.Title = "New user  has been created";
                    tl.EntryType = Operations.New.ToString();
                 
                 
            }

            if (operation == Operations.Edit)
            {
                  tl.Description = "User has been updated! Name :  " + UserFullName;
                  tl.EntryType = Operations.Edit.ToString();
                  tl.Title = "User has been updated";
                  
            }

            if (operation == Operations.Delete)
            {
               
               tl.Description = "User has been deleted! Name :  " + UserFullName;
               tl.Title = "A user has been deleted";
               tl.EntryType = Operations.Delete.ToString();
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();


            context.Clients.All.newEntryTimeLine(tl);
            _adapter.AddNewEntry(tl);
           

        }


        public static void TicketOperations(string ByUser,Operations operation, int ticket_id ,string message="")
        {
            TimeLineLog tl = new TimeLineLog()
            {
               
                Notes = "Notes : " + message == "" ? "No notes" : message,
                TimeHappened = DateTime.Now,
                UserName = ByUser
            };

            if (operation == Operations.New)
            {
                tl.Description = "New Ticket has been created! By :  " + ByUser;
                tl.Title = "New ticket has been created";
                tl.EntryType = Operations.New.ToString();
                
            }

            if (operation == Operations.Edit)
            {
                tl.Description = "Ticket (" + ticket_id + ") has been updated! By :  " + ByUser;
                tl.Title = "A ticket has been updated";
                tl.EntryType = Operations.Edit.ToString();
            }

            if (operation == Operations.Delete)
            {
                tl.Description = "Ticket (" + ticket_id + ") has been deleted! By :  " + ByUser;
                tl.Title = "A user has been deleted";
                tl.EntryType = Operations.Delete.ToString();
            }

            var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();

            context.Clients.All.newEntryTimeLine(tl);
            
            _adapter.AddNewEntry(tl);
            if (operation == Operations.New)
            {
                context.Clients.All.newTicketAdded();
            }
        }


       public static void AssigmentOperations(string ByUser , Operations operation,int issue_id, string staff_name, string message="")
        {
            TimeLineLog tl = new TimeLineLog()
            {

                Notes = "Notes : " + message == "" ? "No notes" : message,
                TimeHappened = DateTime.Now,
                UserName = ByUser
            };

            if (operation == Operations.New)
            {
                tl.Description = "New Assigment has been created! By :  " + ByUser + " , to issue :"+issue_id+" , Tech Name : " +staff_name;
                tl.Title = "Assigment has been created";
                tl.EntryType = Operations.New.ToString();

            }

            if (operation == Operations.Edit)
            {
                tl.Description = "An Assigment has been Updated! By :  " + ByUser + " , issue :" + issue_id + " , Tech Name : " + staff_name;
                tl.Title = "A ticket has been updated";
                tl.EntryType = Operations.Edit.ToString();
            }

            if (operation == Operations.Delete)
            {
                tl.Description = "An Assigment has been deleted! By :  " + ByUser + " , issue :" + issue_id + " , Tech Name : " + staff_name;
                tl.Title = "A user has been deleted";
                tl.EntryType = Operations.Delete.ToString();
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();

            context.Clients.All.newEntryTimeLine(tl);
            _adapter.AddNewEntry(tl);
        }




       internal static void ErrorOperations(string ByUser, Operations operations, string result, int entityId = 0)
       {
           TimeLineLog tl = new TimeLineLog()
           {
               TimeHappened = DateTime.Now,
               UserName = ByUser,
               Description = result,
               EntryId = entityId,
               EntryType = operations.ToString()
           };
           if (operations == Operations.Delete)
           {
               tl.Title = "Error while deleting";
           }
           if (operations == Operations.Edit)
           {
               tl.Title = "Error while Editing/Updating";
           }
           if (operations == Operations.New)
           {
               tl.Title = "Error while Adding";
           }

           var context = GlobalHost.ConnectionManager.GetHubContext<EchoHub>();

           context.Clients.All.newEntryTimeLine(tl);
           _adapter.AddNewEntry(tl);
           
       }
    }

}