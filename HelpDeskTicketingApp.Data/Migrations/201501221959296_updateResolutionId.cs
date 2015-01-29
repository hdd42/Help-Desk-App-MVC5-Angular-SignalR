namespace HelpDeskTicketingApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateResolutionId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "ResolutionId", "dbo.Resolutions");
            DropIndex("dbo.Assignments", new[] { "ResolutionId" });
            AddColumn("dbo.Issues", "ResolutionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Issues", "ResolutionId");
            AddForeignKey("dbo.Issues", "ResolutionId", "dbo.Resolutions", "ResolutionId", cascadeDelete: true);
            DropColumn("dbo.Assignments", "ResolutionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignments", "ResolutionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Issues", "ResolutionId", "dbo.Resolutions");
            DropIndex("dbo.Issues", new[] { "ResolutionId" });
            DropColumn("dbo.Issues", "ResolutionId");
            CreateIndex("dbo.Assignments", "ResolutionId");
            AddForeignKey("dbo.Assignments", "ResolutionId", "dbo.Resolutions", "ResolutionId", cascadeDelete: true);
        }
    }
}
