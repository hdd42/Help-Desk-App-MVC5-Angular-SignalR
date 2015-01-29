using HelpDeskTicketingApp.Data;
using HelpDeskTicketingApp.Data.Models;
using HelpDeskTicketingApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTicketingApp.Adapters.DataAdapters
{
    public class TicketDataAdapter : ITicketAdapter
    {
        private static List<string> HydrateUserTypes()
        {
            List<string> types = new List<string>()
            {
                "Admin",
                "General",
                "Technician"
            };

            return types;
        }

        
        private static List<AdmIssueTypeViewModel> HydrateIssueTypes()
        {
            List<AdmIssueTypeViewModel> issueTypes = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                issueTypes = db.IssueTypes.Select(i => new AdmIssueTypeViewModel
                {
                    IssueTypeDesc = i.IssueTypeDesc,
                    IssueTypeId = i.IssueTypeId
                }).ToList();
            }
            return issueTypes;
        }

        // Return list of users  based on role passed in
        private static List<AdmUserViewModel> HydrateUsers(string role)
        {
            List<AdmUserViewModel> users = new List<AdmUserViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<ApplicationUser> allUsers = db.Users.ToList();
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                // clean up - move creation and all assignment inside of test
                foreach (ApplicationUser u in allUsers)
                {
                    AdmUserViewModel user = new AdmUserViewModel();
                    user.LastName = u.LastName;
                    user.FirstName = u.FirstName;
                    user.Email = u.Email;
                    user.UserId = u.Id;
                    user.UserName = u.UserName;
                    user.Role = userManager.IsInRole(u.Id, "Admin") ? "Admin" : userManager.IsInRole(u.Id, "Technician") ? "Technician" : "General";
                    if (user.Role == role || role == "All")
                    {
                        users.Add(user);
                    }

                }
                return users;
            }
        }

        // Pupulate users based on issue passed in
        public List<AdmUserSelectionModel> HydrateAssignedTechs(int issueId)
        {
            List<AdmUserSelectionModel> techs = new List<AdmUserSelectionModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Assignment> assignments = db.Assignments.Where(a => a.IssueId == issueId).Select(a => a).ToList();
                foreach (Assignment a in assignments)
                {
                    AdmUserSelectionModel tech = new AdmUserSelectionModel();
                    tech.UserName = a.User.FirstName + " " + a.User.LastName;
                    tech.UserId = a.UserId;
                    tech.userEmail = a.User.Email;
                    techs.Add(tech);

                }

            }
            return techs;
        }

        // Populate users based on role passed in
        private static List<AdmUserSelectionModel> HydrateUsersSelection(string role)
        {
            List<AdmUserSelectionModel> users = new List<AdmUserSelectionModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<ApplicationUser> allUsers = db.Users.ToList();
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                foreach (ApplicationUser u in allUsers)
                {
                    if (userManager.IsInRole(u.Id, role) || role == "All")
                    {
                        AdmUserSelectionModel user = new AdmUserSelectionModel();
                        user.UserName = u.FirstName + " " + u.LastName;
                        user.UserId = u.Id;
                        users.Add(user);
                    }

                }

            }
            return users;
        }

        public string GetUserRole(string userId)
        {
            string userRole = null;
            AdmUserViewModel user = GetUserById(userId);
            if (user != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                    userRole = user.Role = userManager.IsInRole(user.UserId, "Admin") ? "Admin" : userManager.IsInRole(user.UserId, "Technician") ? "Technician" : "General"; 
                }
            }

            return userRole;
        }

        public AdmResolutionViewModel GetResolutionById(int id)
        {
            AdmResolutionViewModel resolution = new AdmResolutionViewModel();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Resolution dbResolution = db.Resolutions.FirstOrDefault(r => r.ResolutionId == id);
                if(dbResolution != null)
                {
                    resolution.ResolutionDesc = dbResolution.ResolutionDesc;
                    resolution.ResolutionId = dbResolution.ResolutionId;
                    resolution.IsResolved = dbResolution.IsResolved;
                    resolution.Notes = dbResolution.Notes;
                }
            }
            return resolution;
        }

        public string UpdateResolution(AdmResolutionViewModel resolution)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Resolution dbResolution = db.Resolutions.FirstOrDefault(r => r.ResolutionId == resolution.ResolutionId);
                if (dbResolution != null)
                {
                    dbResolution.ResolutionDesc = resolution.ResolutionDesc;
                    dbResolution.IsResolved = resolution.IsResolved;
                    dbResolution.Notes = resolution.Notes;

                    var issue = db.Issues.FirstOrDefault(i => i.ResolutionId == resolution.ResolutionId);
                    issue.DateResolved = DateTime.Now;

                    var _issueId = issue.IssueId;
                    _username = issue.User.FirstName + " " + issue.User.LastName;
                    _email = issue.User.Email;
                    EmailManager.SendEmailResolvedTicket(_email, _username, _issueId);
                }
           
                try
                {
                    db.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {

                    return "Error while updating resolution(" + resolution.ResolutionId + ")~" + ex.Message;
                }
            }
        }

        // userID can be for technician, admin, or general user - think about renaming
        public List<AdmIssueViewModel> GetUserAssignedIssues(string userId)
        {
            List<AdmIssueViewModel> model = new List<AdmIssueViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // first put in any issues assigned to the user - i.e. this user is a technician
                model = db.Assignments.Where(a => a.UserId == userId).Select(a => new AdmIssueViewModel
                {
                    IssueId = a.IssueId,
                    AssignToId = userId, // Might be able to use this later in display
                    DateReported = a.Issue.DateReported,
                    DateResolved = a.Issue.DateResolved,
                    IsAssigned = a.Issue.IsAssigned,
                    IssueDesc = a.Issue.IssueDesc,
                    IssueType = new AdmIssueTypeViewModel()
                    {
                        IssueTypeDesc = a.Issue.IssueType.IssueTypeDesc,
                        IssueTypeId = a.Issue.IssueType.IssueTypeId

                    },
                    User = new AdmUserViewModel()
                    {
                        FirstName = a.Issue.User.FirstName,
                        LastName = a.Issue.User.LastName,
                        UserId = a.Issue.UserId
                    },
                    Resolution = new AdmResolutionViewModel()
                    {
                        ResolutionDesc = a.Issue.Resolution.ResolutionDesc,
                        ResolutionId = a.Issue.Resolution.ResolutionId,
                        IsResolved = a.Issue.Resolution.IsResolved,
                        Notes = a.Issue.Resolution.Notes
                    }
                }).ToList();                
 
            }
            return model;
        }

        // userID can be for technician, admin, or general user - think about renaming
        public List<AdmIssueViewModel> GetUserReportedIssues(string userId)
        {
            List<AdmIssueViewModel> model = new List<AdmIssueViewModel>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               

                // now put in any issues reported by the user - i.e. this user is an admin, general or technician
                model = db.Issues.Where(i => i.UserId == userId).Select(i => new AdmIssueViewModel
                {
                    IssueId = i.IssueId,
                    AssignToId = userId, // Might be able to use this later in display
                    DateReported = i.DateReported,
                    DateResolved = i.DateResolved,
                    IsAssigned = i.IsAssigned,
                    IssueDesc = i.IssueDesc,
                    IssueType = new AdmIssueTypeViewModel()
                    {
                        IssueTypeDesc = i.IssueType.IssueTypeDesc,
                        IssueTypeId = i.IssueType.IssueTypeId

                    },
                    User = new AdmUserViewModel()
                    {
                        FirstName = i.User.FirstName,
                        LastName = i.User.LastName,
                        UserId = i.UserId
                    }
                }).ToList();
            }
            return model;
        }

        public string GetUserDisplayName(string userId)
        {
            string model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    model = user.FirstName + " " + user.LastName;
                }
            }
            return model;
        }



        public static string GetUserDisplayNameStatic(string userId)
        {
            string model = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    model = user.FirstName + " " + user.LastName;
                }
            }
            return model;
        }


        // Get both issues assigned to user (user is a technician or admin id any exist) and 
        // the issues reported by user (user is technician, admin or general)
        public AdmUserIssuesViewModel GetAllIssuesForSingleUser(string userId)
        {
            AdmUserIssuesViewModel model = new AdmUserIssuesViewModel();
            model.AssignedIssues = GetUserAssignedIssues(userId);
            model.ReportedIssues = GetUserReportedIssues(userId);
            model.UserName = GetUserDisplayName(userId);

            return model;
        }


        public AdmIssuesViewModel GetAllIssues(string status)
        {
            AdmIssuesViewModel model = new AdmIssuesViewModel();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var q = db.Issues.AsQueryable();
                if(status != null)
                {
                    if (status == "Assigned")
                    {
                        q = q.Where(i => i.IsAssigned == true);
                    }
                    else if (status == "UnAssigned")
                    {
                        q = q.Where(i => i.IsAssigned == false);
                    }
                }
                
                model.Issues = q.Select(i => new AdmIssueViewModel
                {
                    IssueId = i.IssueId,
                    DateReported = i.DateReported,
                    DateResolved = i.DateResolved,
                    IsAssigned = i.IsAssigned,
                    IssueDesc = i.IssueDesc,
                    IssueType = new AdmIssueTypeViewModel()
                    {
                        IssueTypeDesc = i.IssueType.IssueTypeDesc,
                        IssueTypeId = i.IssueType.IssueTypeId

                    },
                    User = new AdmUserViewModel()
                    {
                        FirstName = i.User.FirstName,
                        LastName = i.User.LastName,
                        UserId = i.UserId
                    }

                }).ToList();
            }
            return model;
        }

        // Need to 1. Remove resolution, 2. Remove assignments, 3. Remove Ticket
        public string DeleteTicket(int id, string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                Issue issue = db.Issues.FirstOrDefault(i => i.IssueId == id);
                Resolution resolution = db.Resolutions.FirstOrDefault(r => r.ResolutionId == issue.ResolutionId);
                // First remove resolution
                db.Resolutions.Remove(resolution);
                // Now remove any assignments for issue
                List<Assignment> assignments = db.Assignments.Where(a => a.IssueId == issue.IssueId).Select(a => a).ToList();
                foreach (Assignment assignment in assignments)
                {
                    db.Assignments.Remove(assignment);
                }
                // Now we can remove the ticket
                db.Issues.Remove(issue);
                try
                {
                db.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {

                    return "Error while deleting ticket(" + id + ")~" + ex.Message;
        }
        }
        }


        public AdmIssueViewModel GetTicketById(int id)
        {
            AdmIssueViewModel model = new AdmIssueViewModel();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Issue issue = db.Issues.FirstOrDefault(i => i.IssueId == id);
                model.User = new AdmUserViewModel()
                {
                    FirstName = issue.User.FirstName,
                    LastName = issue.User.LastName,
                    UserId = issue.UserId
                };
                model.DateReported = issue.DateReported;
                model.DateResolved = issue.DateResolved;
                model.IsAssigned = issue.IsAssigned;
                model.IssueDesc = issue.IssueDesc;
                model.IssueId = issue.IssueId;
                model.IssueType = new AdmIssueTypeViewModel()
                {
                    IssueTypeDesc = issue.IssueType.IssueTypeDesc,
                    IssueTypeId = issue.IssueType.IssueTypeId
                };
                model.Resolution = new AdmResolutionViewModel()
                {
                    ResolutionId = issue.ResolutionId,
                    Notes = issue.Resolution.Notes,
                    ResolutionDesc = issue.Resolution.ResolutionDesc,
                    //need to find a better way to do this
                    IsResolved = issue.Resolution.IsResolved
                };

            }
            model.IssueTypes = HydrateIssueTypes();
            model.Techs = HydrateUsersSelection("Technician");
            model.Users = HydrateUsersSelection("All");
            model.AssignedTechs = HydrateAssignedTechs(id);
            return model;
        }

        public AdmUsersViewModel GetTechnicians()
        {
            AdmUsersViewModel models = new AdmUsersViewModel();
            models.Users = HydrateUsers("Technician");

            return models;
        }

        public List<AdmUserViewModel> GetUsersByRole(string role)
        {
            List<AdmUserViewModel> models = new List<AdmUserViewModel>();
            models = HydrateUsers(role);

            return models;
        }

        public string UpdateTicket(AdmIssueViewModel model, int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Issue issue = db.Issues.FirstOrDefault(i => i.IssueId == id);
                issue.DateReported = model.DateReported;
                issue.DateResolved = model.DateResolved;
                issue.IssueDesc = model.IssueDesc;
                issue.IssueTypeId = model.IssueType.IssueTypeId;
                issue.UserId = model.User.UserId;
                db.SaveChanges();

                // Get issue again, because we just changed it, and model coming in does not contain user.email
                issue = db.Issues.FirstOrDefault(i => i.IssueId == id);
                string issueId = issue.IssueId.ToString();

                string user_email = issue.User.Email;
                string userName = issue.User.FirstName + " " + issue.User.LastName;
                // email user and tell them that the issue has been updated
                EmailManager.SendReporterEmailAdminUpdateTicket(issueId, userName, user_email, issue.IssueDesc);

                List<AdmUserSelectionModel> techs = HydrateAssignedTechs(id);

                foreach(AdmUserSelectionModel tech in techs)
                {                
                // email all assigned techs and tell them that the issue has been updated
                EmailManager.SendTechEmailAdminUpdateTicket(issueId, tech.UserName, tech.userEmail, issue.IssueDesc);

                }       


                try
                {
                db.SaveChanges();
                    return "ok";
            }
                catch (Exception ex)
                {
                    
                    return "An Error Occured While Updating Ticket : (" + id + ") => " + ex.Message;
        }

            }
   
        }

        // If userId is null, this is being called with the intention of creating a tech
        public AdmUserViewModel GetUserById(string userId)
        {
            AdmUserViewModel model = new AdmUserViewModel();
            if (userId != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.Email = user.Email;
                    model.UserName = user.UserName;
                    model.UserId = user.Id;

                }
            }
            model.Roles = HydrateUserTypes();
            return model;
        }

        // This is used to update/add user, the only differenct is that if it is a create, the userName will be null
        public string AddUpdateUser(AdmUserViewModel model, string userId)
        {
            string message = "";
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user = null;
                if (userId != null) // userName passed in so update existing user
                {
                    user = db.Users.FirstOrDefault(u => u.Id == userId);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;

                    // Find the current role the user is in, and if it is different than the role coming in remove from role and add in corret role
                    string role = userManager.IsInRole(user.Id, "Admin") ? "Admin" : userManager.IsInRole(user.Id, "Technician") ? "Technician" : "General";
                    if (role != model.Role)
                    {
                        userManager.RemoveFromRole(user.Id, role);
                        userManager.AddToRole(user.Id, model.Role);
                    }
                    
                    message = "NewOk";
                }
                else // userName is null - create new user
                {
                    
                    user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email
                    };

                    userManager.Create(user, "123456");
                    userManager.AddToRole(user.Id, model.Role);
                    user = userManager.FindByName(user.UserName);

                    message = "EditOk";
                   
                }
                try
                {
                db.SaveChanges();

                }
                catch (Exception ex)
                {
                    
                    message = "An error occured while adding/updating a user (" + model.FirstName + " " + model.LastName + ") => " + ex.Message;
                }
              
            }
            return message;
        }

        // Need to make sure if role is tech or admin that the assignments are removed
        public string DeleteUser(string userId)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    // if user has reported issues, do not delete them
                    List<AdmIssueViewModel> issues = GetUserReportedIssues(userId);
                    if (issues.Count == 0)
                    {
                        string role = userManager.IsInRole(user.Id, "Admin") ? "Admin" : userManager.IsInRole(user.Id, "Technician") ? "Technician" : "General";
                        // If user is admin or technician, may have assignments - remove the assignments
                        if (role == "Admin" || role == "Technician")
                        {
                            List<Assignment> assignments = db.Assignments.Where(a => a.UserId == userId).Select(a => a).ToList();
                            foreach (Assignment assignment in assignments)
                            {
                                //before removing assignment, see how many techs are assigned to this, if this user is only one set is assigned to false
                                List<AdmUserSelectionModel> techs = HydrateAssignedTechs(assignment.IssueId);
                                if (techs.Count <= 1)
                                {
                                    assignment.Issue.IsAssigned = false;
                                }
                                db.Assignments.Remove(assignment);
                            }
                        }

                        userManager.RemoveFromRole(user.Id, role);
                        userManager.Delete(user);
                    }                   
                }

                try
                {
                    db.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {

                    return "An error occured while deleting user (" + userId + "), => " + ex.Message;
                }

            }

           
        }

        public void AssignTech(AdmIssueViewModel model, int issueId)
        {
            if (model.AssignToId == null)
                return;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                // first see if there is already an assignment with issue and tech, if so don't create another           
                Assignment assignment = db.Assignments.FirstOrDefault(a => a.IssueId == issueId && a.UserId == model.AssignToId);
                if (assignment == null)  // did not find assignment 
                {
                    // create assignment
                    assignment = new Assignment()
                    {
                        IssueId = issueId,
                        UserId = model.AssignToId,
                    };
                    db.Assignments.Add(assignment);

                    Issue issue = db.Issues.FirstOrDefault(i => i.IssueId == issueId);
                    if (issue != null && issue.IsAssigned == false)
                    {
                        issue.IsAssigned = true;
                    }

                    string tech_name = db.Users.Find(model.AssignToId).FirstName;
                    TimeLineHelper.AssigmentOperations(issue.User.UserName, TimeLineHelper.Operations.New, issue.IssueId, tech_name);
                    string ticketId = issue.IssueId.ToString();
                    AdmUserViewModel tech = GetUserById(model.AssignToId);

                    string tech_email = tech.Email;
                    // email user and tell them that the issue has been updated
                    EmailManager.SendEmailAdminAssignTicket(ticketId, tech_email, issue.IssueDesc);
                }

                db.SaveChanges();
            }

            return;
        }






        // added for jims part. i will make the change to reuse the code above once i make sure this is all working 
        string _username;
        string _email;
        public HomeIndexViewModel GetAllUserTickets(string userName)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {


                model.Types = db.IssueTypes.Select(i => new LookUpModel
                {
                    Display = i.IssueTypeDesc,
                    Value = i.IssueTypeId
                }).ToList();


                model.issues = db.Issues.Where(it => it.User.UserName == userName).Select(i => new AdmIssueViewModel
                {


                    DateReported = i.DateReported,
                    DateResolved = i.DateResolved,
                    IssueDesc =i.IssueDesc,
                    IssueType = new AdmIssueTypeViewModel()
                    {
                        IssueTypeId = i.IssueTypeId,
                        IssueTypeDesc = i.IssueDesc
                    },
                    IssueId = i.IssueId,
                    IsAssigned = i.IsAssigned,
                    Resolution = new AdmResolutionViewModel()
                    {
                        ResolutionId = i.ResolutionId,
                        Notes = i.Resolution.Notes,
                        ResolutionDesc = i.Resolution.ResolutionDesc,
                        //need to find a better way to do this
                        IsResolved = db.Resolutions.FirstOrDefault(ri => ri.ResolutionId == i.ResolutionId).IsResolved
                    }

                }).ToList();


            }
            return model;
        }


        public string AddTicket(CreateIssueViewModel model, string userId, string userName)
        {
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //create the res before the issue to stuff the id into the issue when it is made
                Resolution resolution = new Resolution()
                {
                    IsResolved = false
                };

                resolution = db.Resolutions.Add(resolution);
                db.SaveChanges();
                int resolutionId = resolution.ResolutionId;

                //create the issue
              var added =  db.Issues.Add(new Issue
                {

                    DateReported = DateTime.Now,
                    ResolutionId = resolutionId,
                    IssueDesc = model.IssueDescription,
                    UserId = userId,
                    IssueTypeId = model.SelectedIssueTypeId
                });

                var _issueDesc = model.IssueDescription;
                _username = GetUserDisplayName(userId);
                _email = userName;
                EmailManager.SendEmailNewTicket(_email, _username, _issueDesc);

                TimeLineLog tl = new TimeLineLog()
                {
                    Description = "New issue created! Issue on: " + DateTime.Now + " User : " + userName + " , Short Desc: " + model.IssueDescription,
                    EntryType = "new",
                    TimeHappened = DateTime.Now,
                    Title = "New issue created!",
                    UserName = userName
                };
                db.TimeLineLogs.Add(tl);
                
                try
                {
                    db.SaveChanges();
                    return added.IssueId.ToString();
                }
                catch (Exception ex)
                {
                    return "Ann error occured while creating ticket . user : " + _username + " => " + ex.Message;
                    
                }
               
            }
        }

        public string EditTicket(UpdateTicket model, string userId, string userName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var currentTicket = db.Issues.Find(model.IssueId);
                currentTicket.IssueDesc = model.IssueDescription;
                currentTicket.IssueTypeId = model.IssueTypeId;

                var _issueId = model.IssueId;
                var _issueDesc = model.IssueDescription;
                _username = userName;
                _email = userName;
                EmailManager.SendEmailEditTicket(_email, _username, _issueId, _issueDesc);

                TimeLineLog tl = new TimeLineLog()
                {
                    Description = "Issue Edited! Issue on: " + DateTime.Now + " User : " + userName + " , Short Desc: " + model.IssueDescription,
                    EntryType = "new",
                    TimeHappened = DateTime.Now,
                    Title = "Issue Edited!",
                    UserName = userName
                };
                db.TimeLineLogs.Add(tl);
                try
                {
                    db.SaveChanges();
                    return "ok";
                }
                catch (Exception ex)
                {
                    return "An error occured while updating ticket. => " + ex.Message;
                }
                
            }
        }

        public UpdateTicket EditTicket(int id, string name)
        {
            UpdateTicket model = new UpdateTicket();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Types = db.IssueTypes.Select(i => new LookUpModel
                {
                    Display = i.IssueTypeDesc,
                    Value = i.IssueTypeId
                }).ToList();
                //this finds your username and compares it to the one stored in the db so the logged in user gets only their tickets 
                var getTicket = db.Issues.Where(it => it.User.UserName == name).FirstOrDefault(t => t.IssueId == id);
                if (getTicket == null)
                {
                    //sends back to the view page if you edit the url to try to get to someone elses ticket
                    return model;
                }
                else
                {

                    model.IssueDescription = getTicket.IssueDesc;
                    model.IssueId = getTicket.IssueId;
                    model.IssueTypeId = getTicket.IssueTypeId;
                    model.ResolutionId = getTicket.ResolutionId;
                    model.IssueDescription = getTicket.IssueDesc;
                    model.ResolutionDescription = db.Resolutions.FirstOrDefault(r => r.ResolutionId == getTicket.ResolutionId).ResolutionDesc;
                    model.SelectedIssueTypeId = getTicket.IssueTypeId;
                }
            }
            return model;
        }

        public CreateIssueViewModel AddTicket()
        {
            CreateIssueViewModel model = new CreateIssueViewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Types = db.IssueTypes.Select(i => new LookUpModel
                {
                    Display = i.IssueTypeDesc,
                    Value = i.IssueTypeId
                }).ToList();
            }


            return model;
        }

       
    }
}