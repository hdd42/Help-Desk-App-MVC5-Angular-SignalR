using HelpDeskTicketingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using HelpDeskTicketingApp.Data.Models;

namespace HelpDeskTicketingApp.Models
{
    //public class TechnicianViewModels
    //{
    //    public List<AssignmentViewModel> Assignment { get; set; }
    //    public List<AdmUserViewModel> UserList { get; set; }
    //    public List<IssueListModel> IssueList { get; set; } 
    //}

    public class AssignmentViewModel
    {
        public int AssignmentId { get; set; }
        public string UserId { get; set; }
        public int IssueId { get; set; }
        public int ResolutionId { get; set; }
    }

    //public class UserViewModel
    //{
    //    public string UserId { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    
    //}

    public class TechIssueListModel
    {
        public int IssueId { get; set; }
        public AdmIssueTypeViewModel IssueType { get; set; }
        public List<AdmIssueTypeViewModel> IssueTypes { get; set; }
        public AdmUserViewModel User { get; set; }
        public DateTime DateReported { get; set; }
        public DateTime? DateResolved { get; set; }
        public string IssueDesc { get; set; }
        public int ResolutionId { get; set; }

    }

    //public class IssueListModel
    //{
    //    public int IssueId { get; set; }
    //    public int IssueTypeId { get; set; }
    //    public string UserId { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public DateTime DateReported { get; set; }
    //    public DateTime? DateResolved { get; set; }
    //    public string IssueDesc { get; set; }
    //    public bool IsAssigned { get; set; }
    //    public int ResolutionId { get; set; }
    //}
}