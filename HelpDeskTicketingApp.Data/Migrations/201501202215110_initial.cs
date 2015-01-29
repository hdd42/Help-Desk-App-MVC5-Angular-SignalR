namespace HelpDeskTicketingApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        AssignmentId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        IssueId = c.Int(nullable: false),
                        ResolutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignmentId)
                .ForeignKey("dbo.Issues", t => t.IssueId, cascadeDelete: true)
                .ForeignKey("dbo.Resolutions", t => t.ResolutionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.IssueId)
                .Index(t => t.ResolutionId);
            
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        IssueId = c.Int(nullable: false, identity: true),
                        IssueTypeId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateReported = c.DateTime(nullable: false),
                        DateResolved = c.DateTime(nullable: true),
                        IssueDesc = c.String(),
                        IsAssigned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IssueId)
                .ForeignKey("dbo.IssueTypes", t => t.IssueTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.IssueTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IssueTypes",
                c => new
                    {
                        IssueTypeId = c.Int(nullable: false, identity: true),
                        IssueTypeDesc = c.String(),
                    })
                .PrimaryKey(t => t.IssueTypeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Resolutions",
                c => new
                    {
                        ResolutionId = c.Int(nullable: false, identity: true),
                        IsResolved = c.Boolean(nullable: false),
                        ResolutionDesc = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ResolutionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Assignments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assignments", "ResolutionId", "dbo.Resolutions");
            DropForeignKey("dbo.Assignments", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Issues", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "IssueTypeId", "dbo.IssueTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Issues", new[] { "UserId" });
            DropIndex("dbo.Issues", new[] { "IssueTypeId" });
            DropIndex("dbo.Assignments", new[] { "ResolutionId" });
            DropIndex("dbo.Assignments", new[] { "IssueId" });
            DropIndex("dbo.Assignments", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Resolutions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.IssueTypes");
            DropTable("dbo.Issues");
            DropTable("dbo.Assignments");
        }
    }
}
