using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskTicketingApp.Models
{
    //public class HomeViewModel
    //{
    //    public string UserId { get; set; }
    //    public string UserName { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string PhoneNumber { get; set; }

    //}
    //public class IssueViewModel
    //{
        
    //    public int IssueId { get; set; }
    //    public DateTime? DateReported { get; set; }
    //    public DateTime? DateResolved { get; set; }
    //    public AdmIssueTypeViewModel IssueType { get; set; }
    //    //public string IssueType { get; set; }
    //    //public int IssueTypeId { get; set; }
    //    public bool IsAssigned { get; set; }
    //    public AdmResolutionViewModel Resolution { get; set; }
    //    public string IssueDesc { get; set; }
    //    //public bool IsResolved { get; set; }
    //    //public int ResolutionId { get; set; }
        
        
    //}
    public class CreateIssueViewModel
    {
        [Required]
        public string IssueType { get; set; }
        [Required]
        public int IssueTypeId { get; set; }
        public string IssueDescription { get; set; }
        public List<LookUpModel> Types { get; set; }
        public int SelectedIssueTypeId { get; set; }
    }
        
    

    public class LookUpModel
    {
        public int Value { get; set; }
        public string Display { get; set; }
    }
    
 
    public class ResolutionViewModel
    {
        //need the info from two tables for this
        public int ResolutionId { get; set; }
        public int IssueId { get; set; }
        public string ResolutionDescription { get; set; }
        public bool IsResolved { get; set; }
       
    }
    public class UpdateTicket
    {
        public int IssueId { get; set; }
        public string IssueDescription { get; set; }
        public int ResolutionId { get; set; }
        public string ResolutionDescription { get; set; }
        public int IssueTypeId { get; set; }
        public string IssueType { get; set; }
        public List<LookUpModel> Types { get; set; }
        public int SelectedIssueTypeId { get; set; }
    }
    public class HomeIndexViewModel
    {
        public List<AdmIssueViewModel> issues { get; set; }
        public List<ResolutionViewModel> Resolutions { get; set; }
//        public List<HomeViewModel> UserInfo { get; set; }
        public List<LookUpModel> Types { get; set; }
      
    }

    public class ContactIssueViewModel
    {
        [Required]
        public int IssueTypeId { get; set; }
        public string IssueDescription { get; set; }

    }
  
}