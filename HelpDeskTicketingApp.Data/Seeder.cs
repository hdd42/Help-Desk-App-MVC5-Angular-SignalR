using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskTicketingApp.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HelpDeskTicketingApp.Data
{
    public static class Seeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<Role> roleStore = new RoleStore<Role>(db);
            RoleManager<Role> roleManager = new RoleManager<Role>(roleStore);

            if (!roleManager.RoleExists("General"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "General"
                });
            }

            if (!roleManager.RoleExists("Admin"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "Admin"
                });
            }

            if (!roleManager.RoleExists("Technician"))
            {
                var result = roleManager.Create(new Role
                {
                    Name = "Technician"
                });
            }

            ApplicationUser jim = userManager.FindByName("jimmisel@yahoo.com");

            if (jim == null)
            {
                jim = new ApplicationUser
                {
                    Email = "jimmisel@yahoo.com",
                    UserName = "jimmisel@yahoo.com",
                    FirstName = "Jim",
                    LastName = "Misel"
                };

                userManager.Create(jim, "Jmisel1!");
                userManager.AddToRole(jim.Id, "Admin");

                jim = userManager.FindByName("jimmisel@yahoo.com");
            }

            ApplicationUser louis = userManager.FindByName("louism817@gmail.com");

            if (louis == null)
            {
                louis = new ApplicationUser
                {
                    Email = "louism817@gmail.com",
                    UserName = "louism817@gmail.com",
                    FirstName = "Louis",
                    LastName = "Murphy"
                };

                userManager.Create(louis, "Lmurphy1!");
                userManager.AddToRole(louis.Id, "Technician");

                louis = userManager.FindByName("louism817@gmail.com");
            }

            ApplicationUser huseyin = userManager.FindByName("huseyindonmez@live.com");

            if (huseyin == null)
            {
                huseyin = new ApplicationUser
                {
                    Email = "huseyindonmez@live.com",
                    UserName = "huseyindonmez@live.com",
                    FirstName = "Huseyin",
                    LastName = "Donmez"
                };

                userManager.Create(huseyin, "Hdonmez1!");
                userManager.AddToRole(huseyin.Id, "Technician");

                huseyin = userManager.FindByName("huseyindonmez@live.com");
            }

            ApplicationUser coral = userManager.FindByName("c.l.morris@live.com");

            if (coral == null)
            {
                coral = new ApplicationUser
                {
                    Email = "c.l.morris@live.com",
                    UserName = "c.l.morris@live.com",
                    FirstName = "Coral",
                    LastName = "Morris"
                };

                userManager.Create(coral, "Cmorris1!");
                userManager.AddToRole(coral.Id, "Technician");

                coral = userManager.FindByName("c.l.morris@live.com");
            }

            ApplicationUser bob = userManager.FindByName("bob@yahoo.com");

            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    Email = "bob@yahoo.com",
                    UserName = "bob@yahoo.com",
                    FirstName = "Bob",
                    LastName = "TheBuilder"
                };

                userManager.Create(bob, "123456");
                userManager.AddToRole(bob.Id, "General");

                bob = userManager.FindByName("bob@yahoo.com");
            }

            //ApplicationUser customer1 = userManager.FindByName("custo@yahoo.com");

            //if (customer1 == null)
            //{
            //    customer1 = new ApplicationUser
            //    {
            //        Email = "custo@yahoo.com",
            //        UserName = "custo@yahoo.com",
            //        FirstName = "John",
            //        LastName = "Doe"
            //    };

            //    userManager.Create(jim, "Customer1!");
            //   // userManager.AddToRole(jim.Id, "Admin");

            //    customer1 = userManager.FindByName("custo@yahoo.com");
            //}

            db.IssueTypes.AddOrUpdate( i => new { i.IssueTypeDesc},
                new IssueType
                {
                    IssueTypeDesc = "Other"
                },
                new IssueType
                {
                    IssueTypeDesc = "Monitor"
                },
                new IssueType
                {
                    IssueTypeDesc = "Microsoft Office"
                },
                new IssueType
                {
                    IssueTypeDesc = "Wireless Connections"
                },
                new IssueType
                {
                    IssueTypeDesc = "Printer"
                });

        }

    }
}
